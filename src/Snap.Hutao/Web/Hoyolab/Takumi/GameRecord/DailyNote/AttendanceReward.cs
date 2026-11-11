// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Globalization;

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.DailyNote;

internal sealed class AttendanceReward
{
    [JsonPropertyName("status")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AttendanceRewardStatus Status { get; set; }

    public string? StatusFormatted { get => Status.GetLocalizedDescription(SH.ResourceManager, CultureInfo.CurrentCulture); }

    [JsonPropertyName("progress")]
    public int Progress { get; set; }
}