// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Diagnostics;
using Snap.Hutao.Rev.Win32.Foundation;
using System.Runtime.InteropServices;

namespace Snap.Hutao.Rev.Factory.Process;

// 原先由 Snap.Hutao.Native.dll 提供的进程对象，改为直接调用 CreateProcessW。
internal sealed unsafe partial class NativeProcess : IProcess
{
    private const uint StillActive = 259;
    private const uint Infinite = 0xFFFFFFFF;

    private static readonly Lock SyncRoot = new();
    private static readonly EnumWindowsProcedure EnumWindowsCallback = OnEnumWindows;

    private static int searchProcessId;
    private static nint searchResult;

    private nint processHandle;
    private nint threadHandle;

    public NativeProcess(string applicationName, string commandLine, uint creationFlags, string currentDirectory)
    {
        StartupInfo startupInfo = default;
        startupInfo.cb = sizeof(StartupInfo);

        char[] commandLineBuffer = commandLine.ToCharArray();
        fixed (char* pCommandLine = commandLineBuffer)
        {
            if (!CreateProcess(applicationName, pCommandLine, 0, 0, false, creationFlags, 0, currentDirectory, ref startupInfo, out ProcessInformation processInformation))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error(), $"CreateProcess failed: {applicationName}");
            }

            processHandle = processInformation.hProcess;
            threadHandle = processInformation.hThread;
            Id = processInformation.dwProcessId;
        }
    }

    public int Id { get; }

    public nint Handle { get => processHandle; }

    public HWND MainWindowHandle { get => FindWindow(Id); }

    public bool HasExited
    {
        get
        {
            return GetExitCode(out _);
        }
    }

    public int ExitCode
    {
        get
        {
            _ = GetExitCode(out uint code);
            return (int)code;
        }
    }

    public void Start()
    {
        _ = ResumeThread(threadHandle);
    }

    public void ResumeMainThread()
    {
        _ = ResumeThread(threadHandle);
    }

    public void WaitForExit()
    {
        _ = WaitForSingleObject(processHandle, Infinite);
    }

    public void Kill()
    {
        _ = TerminateProcess(processHandle, 1);
    }

    public void Dispose()
    {
        if (processHandle is not 0)
        {
            _ = CloseHandle(processHandle);
            processHandle = 0;
        }

        if (threadHandle is not 0)
        {
            _ = CloseHandle(threadHandle);
            threadHandle = 0;
        }
    }

    private bool GetExitCode(out uint exitCode)
    {
        exitCode = 0;
        if (processHandle is 0 || !Win32GetExitCodeProcess(processHandle, out exitCode))
        {
            return false;
        }

        return exitCode is not StillActive;
    }

    private static HWND FindWindow(int processId)
    {
        lock (SyncRoot)
        {
            searchProcessId = processId;
            searchResult = 0;
            _ = EnumWindows(Marshal.GetFunctionPointerForDelegate(EnumWindowsCallback), 0);
            return searchResult;
        }
    }

    private static bool OnEnumWindows(nint hwnd, nint param)
    {
        _ = GetWindowThreadProcessId(hwnd, out uint owner);
        if (owner != (uint)searchProcessId)
        {
            return true;
        }

        searchResult = hwnd;
        return false;
    }

    private delegate bool EnumWindowsProcedure(nint hwnd, nint param);

    [SuppressMessage("", "SA1307")]
    private struct StartupInfo
    {
        public int cb;
        public nint lpReserved;
        public nint lpDesktop;
        public nint lpTitle;
        public int dwX;
        public int dwY;
        public int dwXSize;
        public int dwYSize;
        public int dwXCountChars;
        public int dwYCountChars;
        public int dwFillAttribute;
        public int dwFlags;
        public short wShowWindow;
        public short cbReserved2;
        public nint lpReserved2;
        public nint hStdInput;
        public nint hStdOutput;
        public nint hStdError;
    }

    [SuppressMessage("", "SA1307")]
    private struct ProcessInformation
    {
        public nint hProcess;
        public nint hThread;
        public int dwProcessId;
        public int dwThreadId;
    }

    [LibraryImport("kernel32.dll", EntryPoint = "CreateProcessW", StringMarshalling = StringMarshalling.Utf16, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool CreateProcess(string? applicationName, char* commandLine, nint processAttributes, nint threadAttributes, [MarshalAs(UnmanagedType.Bool)] bool inheritHandles, uint creationFlags, nint environment, string? currentDirectory, ref StartupInfo startupInfo, out ProcessInformation processInformation);

    [LibraryImport("kernel32.dll", EntryPoint = "ResumeThread", SetLastError = true)]
    private static partial uint ResumeThread(nint thread);

    [LibraryImport("kernel32.dll", EntryPoint = "TerminateProcess", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool TerminateProcess(nint process, uint exitCode);

    [LibraryImport("kernel32.dll", EntryPoint = "WaitForSingleObject", SetLastError = true)]
    private static partial uint WaitForSingleObject(nint handle, uint milliseconds);

    [LibraryImport("kernel32.dll", EntryPoint = "GetExitCodeProcess", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool Win32GetExitCodeProcess(nint process, out uint exitCode);

    [LibraryImport("kernel32.dll", EntryPoint = "CloseHandle", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool CloseHandle(nint handle);

    [LibraryImport("user32.dll", EntryPoint = "EnumWindows", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool EnumWindows(nint callback, nint param);

    [LibraryImport("user32.dll", EntryPoint = "GetWindowThreadProcessId", SetLastError = true)]
    private static partial uint GetWindowThreadProcessId(nint hwnd, out uint processId);
}
