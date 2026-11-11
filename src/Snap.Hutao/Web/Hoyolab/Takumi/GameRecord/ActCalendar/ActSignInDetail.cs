// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.ActCalendar;

internal sealed class ActSignInDetail
{
    [JsonPropertyName("status")]
    public required ActSignInStatus Status { get; init; }

    [JsonPropertyName("progress")]
    public required int Progress { get; init; }

    [JsonPropertyName("total")]
    public required int Total { get; init; }
}