// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Quartz;

namespace Snap.Hutao.Rev.Service.Job;

internal interface IJobScheduler
{
    ValueTask ScheduleAsync(IScheduler scheduler);
}
