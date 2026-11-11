// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Intrinsic;

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.Avatar;

internal class Character : Avatar
{
    [JsonPropertyName("weapon_type")]
    public WeaponType WeaponType { get; set; }

    [JsonPropertyName("weapon")]
    public Weapon Weapon { get; set; } = default!;

    // Ignored field string icon
    // Ignored field string side_icon
}