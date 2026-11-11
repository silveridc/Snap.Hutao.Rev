// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.ActCalendar;

internal sealed class ActDouble : Act
{
    [JsonPropertyName("double_detail")]
    public required ActDoubleDetail DoubleDetail { get; init; }
}