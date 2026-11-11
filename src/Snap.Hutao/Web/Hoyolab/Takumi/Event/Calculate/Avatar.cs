// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Intrinsic;
using Snap.Hutao.Rev.Model.Primitive;

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.Event.Calculate;

internal sealed class Avatar : Calculable
{
    [JsonPropertyName("avatar_level")]
    public QualityType AvatarLevel { get; set; }

    [JsonPropertyName("weapon_cat_id")]
    public WeaponType WeaponCatId { get; set; }

    [JsonPropertyName("element_attr_id")]
    public ElementAttributeId ElementAttrId { get; set; }

    [JsonPropertyName("promote_level")]
    public PromoteLevel PromoteLevel { get; set; }
}