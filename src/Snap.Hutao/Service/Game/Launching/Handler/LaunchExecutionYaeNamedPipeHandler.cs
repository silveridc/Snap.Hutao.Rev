// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core;
using Snap.Hutao.Rev.Core.ExceptionService;
using Snap.Hutao.Rev.Core.LifeCycle.InterProcess.Yae;
using Snap.Hutao.Rev.Service.Game.Island;
using Snap.Hutao.Rev.Service.Game.Launching.Context;
using Snap.Hutao.Rev.Service.Yae.Achievement;
using Snap.Hutao.Rev.Win32.Foundation;
using System.IO;

namespace Snap.Hutao.Rev.Service.Game.Launching.Handler;

internal sealed class LaunchExecutionYaeNamedPipeHandler : AbstractLaunchExecutionHandler
{
    private readonly TargetNativeConfiguration config;
    private readonly YaeDataArrayReceiver receiver;

    public LaunchExecutionYaeNamedPipeHandler(TargetNativeConfiguration config, YaeDataArrayReceiver receiver)
    {
        this.config = config;
        this.receiver = receiver;
    }

    public override async ValueTask ExecuteAsync(LaunchExecutionContext context)
    {
        if (!HutaoRuntime.IsProcessElevated)
        {
            context.Process.Kill();
            HutaoException.NotSupported(SH.ServiceGameLaunchingHandlerEmbeddedYaeClientNotElevated);
        }

        if (!context.LaunchOptions.IsSetTargetFrameRateEnabled.Value)
        {
            context.Process.Kill();
            HutaoException.NotSupported(SH.ServiceGameLaunchingHandlerEmbeddedYaeIslandNotEnabled);
            return;
        }

        string dataFolderYaePath = Path.Combine(HutaoRuntime.DataDirectory, "YaeAchievementLib.dll");
        InstalledLocation.CopyFileFromApplicationUri("ms-appx:///Resource/Yae/YaeAchievementLib.dll", dataFolderYaePath);

        try
        {
            DllInjectionUtilities.InjectUsingRemoteThread(dataFolderYaePath, "YaeMain", context.Process.Id);
        }
        catch (Exception ex)
        {
            // Windows Defender Application Control
            if (HRESULT.IsWin32(ex.HResult, WIN32_ERROR.ERROR_SYSTEM_INTEGRITY_POLICY_VIOLATION))
            {
                context.Process.Kill();
                throw HutaoException.Throw(SH.ServiceGameLaunchingHandlerEmbeddedYaeErrorSystemIntegrityPolicyViolation);
            }

            throw;
        }

        try
        {
#pragma warning disable CA2007
            await using (YaeNamedPipeServer server = new(context.ServiceProvider, context.Process, config))
#pragma warning restore CA2007
            {
                receiver.Array = await server.GetDataArrayAsync().ConfigureAwait(false);
            }
        }
        catch (Exception)
        {
            context.Process.Kill();
            throw;
        }
    }
}