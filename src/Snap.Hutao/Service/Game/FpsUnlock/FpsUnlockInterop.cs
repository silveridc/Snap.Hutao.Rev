// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core;
using Snap.Hutao.Rev.Core.ExceptionService;
using Snap.Hutao.Rev.Core.LifeCycle.InterProcess.FullTrust;
using Snap.Hutao.Rev.Core.Setting;
using Snap.Hutao.Rev.Factory.Process;
using Snap.Hutao.Rev.Service.Game.Launching.Context;
using System.IO;
using System.IO.MemoryMappedFiles;
using Windows.Devices.Input;

namespace Snap.Hutao.Rev.Service.Game.FpsUnlock;

internal static class FpsUnlockInterop
{
    // Creating a Global\ mapping needs SeCreateGlobalPrivilege, which the launcher token
    // does not have. The module opens this mapping without the prefix, keep it session local.
    private const string SharedMemoryName = "2DE95FDC-6AB7-4593-BFE6-760DD4AB422B";

    public static void Before(BeforeLaunchExecutionContext context)
    {
        // Extract the stub next to the data directory. The helper waits for the game
        // window and injects it via a WH_GETMESSAGE hook.
        string stubTargetPath = HutaoRuntime.GetDataDirectoryFile("UnlockerStub.dll");
        InstalledLocation.CopyFileFromApplicationUri("ms-appx:///Resource/FpsUnlock/UnlockerStub.dll", stubTargetPath);
    }

    public static async ValueTask RunAsync(LaunchExecutionContext context, CancellationToken token = default)
    {
        if (context.Process is not FullTrustProcess fullTrustProcess)
        {
            throw HutaoException.InvalidOperation("FpsUnlock requires the full trust launcher process");
        }

        string stubPath = HutaoRuntime.GetDataDirectoryFile("UnlockerStub.dll");
        if (!File.Exists(stubPath))
        {
            throw HutaoException.InvalidOperation("UnlockerStub.dll not found");
        }

        // Create the shared memory BEFORE the stub's DllMain reads it.
        using (MemoryMappedFile file = MemoryMappedFile.CreateOrOpen(SharedMemoryName, 4096, MemoryMappedFileAccess.ReadWrite))
        using (MemoryMappedViewAccessor accessor = file.CreateViewAccessor())
        {
            InitializeIpcData(accessor, context.LaunchOptions);
            fullTrustProcess.LoadLibrary(FullTrustLoadLibraryRequest.Create("FpsUnlock", stubPath));
            await WaitForReadyAsync(accessor, token).ConfigureAwait(false);
        }

        // The stub reads the whole IPC struct from shared memory every 62ms.
        // We keep writing the fields it can act on at runtime so changes are picked up
        // without touching Status at offset 0, which the stub owns.
        using (MemoryMappedFile file = MemoryMappedFile.OpenExisting(SharedMemoryName, MemoryMappedFileRights.ReadWrite))
        using (MemoryMappedViewAccessor accessor = file.CreateViewAccessor())
        {
            while (!context.Process.HasExited && !token.IsCancellationRequested)
            {
                UpdateIpcData(accessor, context.LaunchOptions);
                await Task.Delay(500, token).ConfigureAwait(false);
            }
        }
    }

    private static async ValueTask WaitForReadyAsync(MemoryMappedViewAccessor accessor, CancellationToken token)
    {
        for (int retryCount = 0; retryCount < 20; retryCount++)
        {
            accessor.Read(0, out FpsUnlockIpcData data);
            if (data.Status is 2)
            {
                return;
            }

            if (data.Status is 1)
            {
                throw HutaoException.InvalidOperation("FpsUnlock stub initialization failed");
            }

            await Task.Delay(1000, token).ConfigureAwait(false);
        }

        throw HutaoException.InvalidOperation("FpsUnlock stub initialization timed out");
    }

    private static void InitializeIpcData(MemoryMappedViewAccessor accessor, LaunchOptions options)
    {
        FpsUnlockIpcData data = new()
        {
            Framerate = GetTargetFramerate(options),
            PowerSave = options.IsPowerSaveEnabled.Value ? (byte)1 : (byte)0,
            UseMobileUI = ShouldUseMobileUI(options) ? (byte)1 : (byte)0,
        };

        accessor.Write(0, ref data);
    }

    private static bool ShouldUseMobileUI(LaunchOptions options)
    {
        if (options.IsIslandEnabled.Value)
        {
            // The injected module switches the input device itself, the stub must not fight it.
            return false;
        }

        return LocalSetting.Get(SettingKeys.LaunchForceUsingTouchScreen, false)
            ? IsIntegratedTouchPresent()
            : options.UsingTouchScreen.Value;
    }

    private static bool IsIntegratedTouchPresent()
    {
        IReadOnlyList<PointerDevice> devices = PointerDevice.GetPointerDevices();

        // ReSharper disable once ForCanBeConvertedToForeach
        // https://github.com/microsoft/CsWinRT/issues/747
        for (int i = 0; i < devices.Count; i++)
        {
            PointerDevice device = devices[i];
            if (device is { PointerDeviceType: PointerDeviceType.Touch, IsIntegrated: true })
            {
                return true;
            }
        }

        return false;
    }

    private static void UpdateIpcData(MemoryMappedViewAccessor accessor, LaunchOptions options)
    {
        accessor.Write(4, GetTargetFramerate(options));
        accessor.Write(8, options.IsPowerSaveEnabled.Value ? (byte)1 : (byte)0);
    }

    private static int GetTargetFramerate(LaunchOptions options)
    {
        return options.IsSetTargetFrameRateEnabled.Value
            ? Math.Clamp(options.TargetFps.Value, 10, 1000)
            : options.TargetFps.Value;
    }
}
