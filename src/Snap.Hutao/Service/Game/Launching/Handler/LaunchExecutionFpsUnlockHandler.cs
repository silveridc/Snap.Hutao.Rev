// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Service.Game.FpsUnlock;
using Snap.Hutao.Rev.Service.Game.Launching.Context;
using Snap.Hutao.Rev.Service.Notification;

namespace Snap.Hutao.Rev.Service.Game.Launching.Handler;

internal sealed class LaunchExecutionFpsUnlockHandler : AbstractLaunchExecutionHandler
{
    public override ValueTask BeforeAsync(BeforeLaunchExecutionContext context)
    {
        if (context.LaunchOptions.IsSetTargetFrameRateEnabled.Value)
        {
            FpsUnlockInterop.Before(context);
        }

        return ValueTask.CompletedTask;
    }

    public override ValueTask ExecuteAsync(LaunchExecutionContext context)
    {
        if (!context.LaunchOptions.IsSetTargetFrameRateEnabled.Value)
        {
            return ValueTask.CompletedTask;
        }

        ExecuteCoreAsync(context).SafeForget();
        return ValueTask.CompletedTask;
    }

    private async ValueTask ExecuteCoreAsync(LaunchExecutionContext context)
    {
        try
        {
            await FpsUnlockInterop.RunAsync(context).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // The game itself is unaffected by a failed injection, leave it running.
            context.Messenger.Send(InfoBarMessage.Error(ex));
        }
    }
}
