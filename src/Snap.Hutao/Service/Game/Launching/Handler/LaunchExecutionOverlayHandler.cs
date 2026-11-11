// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core;
using Snap.Hutao.Rev.Service.Game.Launching.Context;
using Snap.Hutao.Rev.UI.Xaml.View.Window;

namespace Snap.Hutao.Rev.Service.Game.Launching.Handler;

internal sealed class LaunchExecutionOverlayHandler : AbstractLaunchExecutionHandler
{
    private LaunchExecutionOverlayWindow? window;

    public override async ValueTask ExecuteAsync(LaunchExecutionContext context)
    {
        if (!HutaoRuntime.IsProcessElevated || !context.LaunchOptions.UsingOverlay.Value)
        {
            return;
        }

        await context.TaskContext.SwitchToMainThreadAsync();
        window = context.ServiceProvider.GetRequiredService<LaunchExecutionOverlayWindow>();
    }

    public override async ValueTask AfterAsync(AfterLaunchExecutionContext context)
    {
        if (window is null)
        {
            return;
        }

        await context.TaskContext.SwitchToMainThreadAsync();
        window.PreventClose = false;
        window.Close();
    }
}