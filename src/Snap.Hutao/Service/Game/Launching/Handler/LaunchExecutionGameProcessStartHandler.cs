// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.
using Snap.Hutao.Rev.Core;
using Snap.Hutao.Rev.Service.Game.Launching.Context;
using Snap.Hutao.Rev.Win32.Foundation;

namespace Snap.Hutao.Rev.Service.Game.Launching.Handler;

internal sealed class LaunchExecutionGameProcessStartHandler : AbstractLaunchExecutionHandler
{
    public override async ValueTask BeforeAsync(BeforeLaunchExecutionContext context)
    {
        LaunchOptions launchOptions = context.LaunchOptions;
        if (launchOptions.IsSetTargetFrameRateEnabled.Value || launchOptions.IsIslandEnabled.Value)
        {
            string fullTrustTargetPath = HutaoRuntime.GetDataDirectoryFile("Snap.Hutao.FullTrust.FpsV3.exe");
            if (!System.IO.File.Exists(fullTrustTargetPath))
            {
                InstalledLocation.CopyFileFromApplicationUri("ms-appx:///Resource/FpsUnlock/Snap.Hutao.FullTrust.exe", fullTrustTargetPath);
            }
        }
    }

    public override async ValueTask ExecuteAsync(LaunchExecutionContext context)
    {
        try
        {
            context.Process.Start();
            await context.TaskContext.SwitchToMainThreadAsync();
            GameLifeCycle.IsGameRunningProperty.Value = true;
        }
        catch (Win32Exception ex)
        {
            if (ex.HResult is HRESULT.E_FAIL)
            {
                return;
            }

            throw;
        }

        context.Progress.Report(new(SH.ServiceGameLaunchPhaseProcessStarted));
    }
}