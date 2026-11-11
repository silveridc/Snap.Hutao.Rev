// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Diagnostics;
using Snap.Hutao.Rev.Core.LifeCycle.InterProcess.Yae;
using Snap.Hutao.Rev.Service.Game.Launching.Context;
using Snap.Hutao.Rev.Service.Game.Launching.Handler;
using Snap.Hutao.Rev.Service.Yae.Achievement;

namespace Snap.Hutao.Rev.Service.Game.Launching.Invoker;

internal sealed class YaeLaunchExecutionInvoker : AbstractLaunchExecutionInvoker
{
    public YaeLaunchExecutionInvoker(TargetNativeConfiguration config, YaeDataArrayReceiver receiver)
    {
        Handlers =
        [
            new LaunchExecutionGameLifeCycleHandler(resume: false),
            new LaunchExecutionChannelOptionsHandler(),
            new LaunchExecutionGameResourceHandler(false),
            new LaunchExecutionGameIdentityHandler(),
            new LaunchExecutionGameProcessStartHandler(),
            new LaunchExecutionYaeNamedPipeHandler(config, receiver),
        ];
    }

    protected override IProcess? CreateProcess(BeforeLaunchExecutionContext beforeContext)
    {
        return GameProcessFactory.CreateForEmbeddedYae(beforeContext);
    }
}