// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Service.Game.Island;
using Snap.Hutao.Rev.Service.Game.Launching.Context;
using Snap.Hutao.Rev.Service.Notification;

namespace Snap.Hutao.Rev.Service.Game.Launching.Handler;

internal sealed class LaunchExecutionIslandHandler : AbstractLaunchExecutionHandler
{
    private readonly bool resume;

    public LaunchExecutionIslandHandler(bool resume)
    {
        this.resume = resume;
    }

    public override ValueTask BeforeAsync(BeforeLaunchExecutionContext context)
    {
        if (!resume && context.LaunchOptions.IsIslandEnabled.Value)
        {
            IslandInterop.Before();
        }

        return ValueTask.CompletedTask;
    }

    public override ValueTask ExecuteAsync(LaunchExecutionContext context)
    {
        if (!context.LaunchOptions.IsIslandEnabled.Value)
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
            await IslandInterop.RunAsync(context, resume).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // The game itself is unaffected by a failed injection, leave it running.
            context.Messenger.Send(InfoBarMessage.Error(ex));
        }
    }
}
