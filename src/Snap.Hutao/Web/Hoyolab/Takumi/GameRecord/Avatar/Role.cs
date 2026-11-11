// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.Avatar;

internal sealed class Role
{
    [JsonPropertyName("AvatarUrl")]
    public string AvatarUrl { get; set; } = default!;

    [JsonPropertyName("nickname")]
    public string Nickname { get; set; } = default!;

    [JsonPropertyName("region")]
    public string RegionName { get; set; } = default!;

    [JsonPropertyName("level")]
    public int Level { get; set; } = default!;
}