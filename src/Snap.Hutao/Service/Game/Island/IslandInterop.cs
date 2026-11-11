// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core;
using Snap.Hutao.Rev.Core.ExceptionService;
using Snap.Hutao.Rev.Core.LifeCycle.InterProcess.FullTrust;
using Snap.Hutao.Rev.Factory.Process;
using Snap.Hutao.Rev.Service.Game.Launching.Context;
using Snap.Hutao.Rev.UI.Input.LowLevel;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace Snap.Hutao.Rev.Service.Game.Island;

internal static class IslandInterop
{
    private const string DllName = "Snap.Hutao.Remastered.UnlockerIsland.dll";
    private const string DllApplicationUri = "ms-appx:///Resource/Island/Snap.Hutao.Remastered.UnlockerIsland.dll";

    // The module opens this mapping without the Global\ prefix, keep it session local.
    private const string SharedMemoryName = "4F3E8543-40F7-4808-82DC-21E48A6037A7";

    public static void Before()
    {
        string dllPath = HutaoRuntime.GetDataDirectoryFile(DllName);
        InstalledLocation.CopyFileFromApplicationUri(DllApplicationUri, dllPath);
        HutaoException.ThrowIf(!File.Exists(dllPath), SH.ServiceGameIslandDllNotFound);

        // Fail before the game is created suspended for the injection.
        HutaoException.ThrowIf(!IsVisualCppRuntimeInstalled(), SH.ServiceGameIslandRuntimeNotInstalled);
    }

    public static async ValueTask RunAsync(LaunchExecutionContext context, bool resume, CancellationToken token = default)
    {
        string dllPath = HutaoRuntime.GetDataDirectoryFile(DllName);
        MemoryMappedFile file;
        if (resume)
        {
            try
            {
                file = MemoryMappedFile.OpenExisting(SharedMemoryName, MemoryMappedFileRights.ReadWrite);
            }
            catch (FileNotFoundException)
            {
                // The running game has no module injected, there is nothing to attach to.
                return;
            }
        }
        else
        {
            file = MemoryMappedFile.CreateOrOpen(SharedMemoryName, 4096, MemoryMappedFileAccess.ReadWrite);
        }

        using (file)
        {
            using MemoryMappedViewAccessor accessor = file.CreateViewAccessor();
            LowLevelKeyOptions keyOptions = context.ServiceProvider.GetRequiredService<LowLevelKeyOptions>();

            // The module crashes on attach when the mapping is missing, create and fill it first.
            accessor.Write(IslandHookEnvironment.IsOverseaOffset, IslandHookEnvironment.ToBool(context.IsOversea));

            // Let the module resolve function addresses from its own signature patterns.
            accessor.Write(IslandHookEnvironment.ProvideOffsetsOffset, 0);
            IslandHookEnvironment.WriteFlags(accessor, context.LaunchOptions, GetCombineHotkey(keyOptions));

            if (!resume)
            {
                // The game is suspended until the module is queued, injecting later would
                // race the module's own initialization against the game's.
                LoadLibraryAndResume(context, dllPath);
                await WaitForStartedAsync(accessor, token).ConfigureAwait(false);
            }

            SetConnected(context.TaskContext, true);

            try
            {
                while (!context.Process.HasExited && !token.IsCancellationRequested)
                {
                    IslandHookEnvironment.WriteFlags(accessor, context.LaunchOptions, GetCombineHotkey(keyOptions));
                    await Task.Delay(500, token).ConfigureAwait(false);
                }
            }
            finally
            {
                SetConnected(context.TaskContext, false);
            }
        }
    }

    private static void LoadLibraryAndResume(LaunchExecutionContext context, string dllPath)
    {
        if (context.Process is not FullTrustProcess fullTrustProcess)
        {
            throw HutaoException.InvalidOperation("Island requires the full trust launcher process");
        }

        try
        {
            fullTrustProcess.LoadLibrary(FullTrustLoadLibraryRequest.Create("Island", dllPath));
        }
        catch
        {
            // Never leave the game suspended when the injection is refused.
            fullTrustProcess.ResumeMainThread();
            throw;
        }

        fullTrustProcess.ResumeMainThread();
    }

    private static bool IsVisualCppRuntimeInstalled()
    {
        string systemDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System);
        return File.Exists(Path.Combine(systemDirectory, "vcruntime140.dll"))
            && File.Exists(Path.Combine(systemDirectory, "vcruntime140_1.dll"))
            && File.Exists(Path.Combine(systemDirectory, "msvcp140.dll"));
    }

    private static int GetCombineHotkey(LowLevelKeyOptions keyOptions)
    {
        return (int)keyOptions.IslandCombineKey.Value;
    }

    private static void SetConnected(ITaskContext taskContext, bool connected)
    {
        taskContext.InvokeOnMainThread(() => LaunchOptions.IsIslandConnected.Value = connected);
    }

    private static async ValueTask WaitForStartedAsync(MemoryMappedViewAccessor accessor, CancellationToken token)
    {
        for (int retryCount = 0; retryCount < 60; retryCount++)
        {
            IslandState state = (IslandState)accessor.ReadInt32(IslandHookEnvironment.StateOffset);
            if (state is IslandState.Started)
            {
                return;
            }

            if (state is IslandState.Error)
            {
                uint lastError = accessor.ReadUInt32(IslandHookEnvironment.LastErrorOffset);
                throw HutaoException.InvalidOperation($"{SH.ServiceGameIslandInitializationFailed} 0x{lastError:X8}");
            }

            await Task.Delay(1000, token).ConfigureAwait(false);
        }

        throw HutaoException.InvalidOperation(SH.ServiceGameIslandInitializationTimeout);
    }
}
