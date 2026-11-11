// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.ActCalendar;

[ExtendedEnum]
internal enum ActStatus
{
    [LocalizationKey(nameof(SH.WebHoyolabGameRecordActCalendarActStatusUpcoming))]
    Upcoming = 1,

    [LocalizationKey(nameof(SH.WebHoyolabGameRecordActCalendarActStatusOngoing))]
    Ongoing = 2,
}