// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.RoleCombat;

internal sealed class RoleCombatData
{
    [JsonPropertyName("detail")]
    public required RoleCombatDetail Detail { get; init; }

    [JsonPropertyName("stat")]
    public required RoleCombatStat Stat { get; init; }

    [JsonPropertyName("schedule")]
    public required RoleCombatSchedule Schedule { get; init; }

    [JsonPropertyName("has_data")]
    public required bool HasData { get; init; }

    [JsonPropertyName("has_detail_data")]
    public required bool HasDetailData { get; init; }
}