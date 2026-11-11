// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Intrinsic;
using Snap.Hutao.Rev.Model.Primitive;

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.Avatar;

internal class Weapon
{
    [JsonPropertyName("id")]
    public WeaponId Id { get; set; }

    [JsonPropertyName("type")]
    public WeaponType Type { get; set; }

    [JsonPropertyName("rarity")]
    public QualityType Rarity { get; set; }

    [JsonPropertyName("level")]
    public Level Level { get; set; }

    [JsonPropertyName("affix_level")]
    public uint AffixLevel { get; set; }

    // Ignored field string icon
}
