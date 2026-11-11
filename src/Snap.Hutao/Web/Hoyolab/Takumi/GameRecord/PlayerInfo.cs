// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord;

internal sealed class PlayerInfo
{
    [JsonPropertyName("role")]
    public BasicRoleInfo Role { get; set; } = default!;

    [JsonPropertyName("stats")]
    public PlayerStats PlayerStat { get; set; } = default!;

    [JsonPropertyName("world_explorations")]
    public List<WorldExploration> WorldExplorations { get; set; } = default!;

    [JsonPropertyName("homes")]
    public List<Home> Homes { get; set; } = default!;

    // Ignored
    // [JsonPropertyName("avatars")]
    // public List<Avatar.Avatar> Avatars { get; set; } = default!;
}