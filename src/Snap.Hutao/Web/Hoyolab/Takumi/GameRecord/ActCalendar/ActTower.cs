// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.ActCalendar;

internal sealed class ActTower : Act
{
    [JsonPropertyName("tower_detail")]
    public required ActTowerDetail TowerDetail { get; init; }
}