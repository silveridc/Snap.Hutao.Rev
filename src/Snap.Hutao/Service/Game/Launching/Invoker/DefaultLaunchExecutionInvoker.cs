// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Service.Game.Launching.Handler;

namespace Snap.Hutao.Rev.Service.Game.Launching.Invoker;

internal sealed class DefaultLaunchExecutionInvoker : AbstractLaunchExecutionInvoker
{
    public DefaultLaunchExecutionInvoker()
    {
        Handlers =
        [
            new LaunchExecutionGameLifeCycleHandler(resume: false),
            new LaunchExecutionChannelOptionsHandler(),
            new LaunchExecutionGameResourceHandler(convertOnly: false),
            new LaunchExecutionGameIdentityHandler(),
            new LaunchExecutionWindowsHDRHandler(),
            new LaunchExecutionGameProcessStartHandler(),
            new LaunchExecutionFpsUnlockHandler(),
            new LaunchExecutionIslandHandler(resume: false),
            new LaunchExecutionOverlayHandler(),
            new LaunchExecutionStarwardPlayTimeStatisticsHandler(),
            new LaunchExecutionBetterGenshinImpactAutomationHandler()
        ];
    }
}