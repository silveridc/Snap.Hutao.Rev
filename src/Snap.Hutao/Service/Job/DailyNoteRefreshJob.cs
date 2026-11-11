// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Quartz;
using Snap.Hutao.Rev.Service.DailyNote;

namespace Snap.Hutao.Rev.Service.Job;

internal sealed partial class DailyNoteRefreshJob : IJob
{
    private readonly IDailyNoteService dailyNoteService;

    [GeneratedConstructor]
    public partial DailyNoteRefreshJob(IServiceProvider serviceProvider);

    [SuppressMessage("", "SH003")]
    public async ValueTask Execute(IJobExecutionContext context, CancellationToken cancellationToken)
    {
        await dailyNoteService.RefreshDailyNotesAsync(cancellationToken).ConfigureAwait(false);
    }
}