// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Entity;

namespace Snap.Hutao.Rev.Service.DailyNote.NotifySuppression;

internal sealed class NotifySuppressionContext : INotifySuppressionContext
{
    private readonly List<DailyNoteNotifyInfo> infos;

    public NotifySuppressionContext(DailyNoteEntry entry, List<DailyNoteNotifyInfo> infos)
    {
        Entry = entry;
        this.infos = infos;
    }

    public DailyNoteEntry Entry { get; }

    public void Add(DailyNoteNotifyInfo info)
    {
        infos.Add(info);
    }
}