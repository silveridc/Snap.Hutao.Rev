// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.ExceptionService;
using Snap.Hutao.Rev.Service.Game.Launching.Context;

namespace Snap.Hutao.Rev.Service.Game.Launching.Handler;

internal sealed class LaunchExecutionGameLifeCycleHandler : AbstractLaunchExecutionHandler
{
    private readonly bool resume;

    public LaunchExecutionGameLifeCycleHandler(bool resume)
    {
        this.resume = resume;
    }

    public override async ValueTask BeforeAsync(BeforeLaunchExecutionContext context)
    {
        if (resume)
        {
            if (await GameLifeCycle.TryGetRunningGameProcessAsync(context.TaskContext).ConfigureAwait(false) is (true, { } gameProcess))
            {
                context.SetOption(LaunchExecutionOptionsKey.RunningProcess, gameProcess);
            }
        }
        else
        {
            if (await GameLifeCycle.IsGameRunningAsync(context.TaskContext).ConfigureAwait(false))
            {
                HutaoException.Throw(SH.ServiceGameLaunchExecutionGameIsRunning);
            }
        }
    }
}