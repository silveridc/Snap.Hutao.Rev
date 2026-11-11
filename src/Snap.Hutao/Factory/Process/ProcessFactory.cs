// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core;
using Snap.Hutao.Rev.Core.Diagnostics;
using Snap.Hutao.Rev.Core.LifeCycle.InterProcess.FullTrust;
using Snap.Hutao.Rev.Win32.Foundation;

namespace Snap.Hutao.Rev.Factory.Process;

internal sealed class ProcessFactory
{
    public static void KillCurrent()
    {
        global::System.Diagnostics.Process.GetCurrentProcess().Kill();
    }

    public static bool TryGetById(int processId, [NotNullWhen(true)] out IProcess? process)
    {
        try
        {
            process = new DiagnosticsProcess(global::System.Diagnostics.Process.GetProcessById(processId));
            return true;
        }
        catch (Exception ex)
        {
            // Process with an Id of $id$ is not running.
            if (ex is not ArgumentException)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        process = null;
        return false;
    }

    public static bool IsRunning(ReadOnlySpan<string> processNames, [NotNullWhen(true)] out IProcess? runningProcess)
    {
        int currentSessionId = global::System.Diagnostics.Process.GetCurrentProcess().SessionId;
        global::System.Diagnostics.Process[] processes = global::System.Diagnostics.Process.GetProcesses();

        // GetProcesses once and manually loop is O(n)
        foreach (global::System.Diagnostics.Process process in processes)
        {
            try
            {
                if (process.SessionId != currentSessionId)
                {
                    continue;
                }

                if (!process.ProcessName.EqualsAny(processNames, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // Force access handle to check whether process has exited
                try
                {
                    _ = process.Handle;
                }
                catch (Exception ex)
                {
                    // Assume it's running if access is denied.
                    if (!HRESULT.IsWin32(ex, WIN32_ERROR.ERROR_ACCESS_DENIED))
                    {
                        throw;
                    }
                }

                runningProcess = new DiagnosticsProcess(process);
                return true;
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    // 拒绝访问。
                    case Win32Exception we:
                        if (we.NativeErrorCode is (int)WIN32_ERROR.ERROR_ACCESS_DENIED)
                        {
                            runningProcess = new DiagnosticsProcess(process);
                            return true;
                        }

                        break;

                    // Cannot process request because the process ($id$) has exited.
                    case InvalidOperationException ioe:
                        runningProcess = default;
                        return false;
                }

                SentrySdk.CaptureException(ex);
                break;
            }
        }

        runningProcess = default;
        return false;
    }

    public static IProcess CreateUsingShellExecuteRunAs(string arguments, string fileName, string workingDirectory)
    {
        global::System.Diagnostics.Process process = new()
        {
            StartInfo = new()
            {
                Arguments = arguments,
                FileName = fileName,
                UseShellExecute = true,
                Verb = "runas",
                WorkingDirectory = workingDirectory,
            },
        };

        return new DiagnosticsProcess(process);
    }

    public static IProcess CreateSuspended(string arguments, string fileName, string workingDirectory)
    {
        return new NativeProcess(fileName, arguments, (uint)Win32.System.Threading.PROCESS_CREATION_FLAGS.CREATE_SUSPENDED, workingDirectory);
    }

    public static IProcess CreateUsingFullTrust(string arguments, string fileName, string workingDirectory)
    {
        string fullTrustFilePath = HutaoRuntime.GetDataDirectoryFile("Snap.Hutao.FullTrust.FpsV3.exe");
        StartUsingShellExecuteRunAs(fullTrustFilePath);

        FullTrustProcessStartInfoRequest request = new()
        {
            ApplicationName = fileName,
            CommandLine = arguments,
            CreationFlags = 0,
            CurrentDirectory = workingDirectory,
        };

        return new FullTrustProcess(request);
    }

    public static IProcess CreateUsingFullTrustSuspended(string arguments, string fileName, string workingDirectory)
    {
        string fullTrustFilePath = HutaoRuntime.GetDataDirectoryFile("Snap.Hutao.FullTrust.FpsV3.exe");
        StartUsingShellExecuteRunAs(fullTrustFilePath);

        FullTrustProcessStartInfoRequest request = new()
        {
            ApplicationName = fileName,
            CommandLine = arguments,
            CreationFlags = Win32.System.Threading.PROCESS_CREATION_FLAGS.CREATE_SUSPENDED,
            CurrentDirectory = workingDirectory,
        };

        return new FullTrustProcess(request);
    }

    public static void StartUsingShellExecute(string arguments, string fileName)
    {
        global::System.Diagnostics.Process.Start(new global::System.Diagnostics.ProcessStartInfo
        {
            Arguments = arguments,
            FileName = fileName,
            UseShellExecute = true,
        });
    }

    public static void StartUsingShellExecuteRunAs(string fileName)
    {
        global::System.Diagnostics.Process.Start(new global::System.Diagnostics.ProcessStartInfo
        {
            FileName = fileName,
            UseShellExecute = true,
            Verb = "runas",
        });
    }
}