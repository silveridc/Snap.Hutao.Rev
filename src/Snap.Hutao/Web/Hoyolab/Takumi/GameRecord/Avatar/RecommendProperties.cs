// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Intrinsic;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.Avatar;

internal sealed class RecommendProperties
{
    [JsonPropertyName("sand_main_property_list")]
    public ImmutableArray<FightProperty> SandMainPropertyList { get; set; }

    [JsonPropertyName("goblet_main_property_list")]
    public ImmutableArray<FightProperty> GobletMainPropertyList { get; set; }

    [JsonPropertyName("circlet_main_property_list")]
    public ImmutableArray<FightProperty> CircletMainPropertyList { get; set; }

    [JsonPropertyName("sub_property_list")]
    public ImmutableArray<FightProperty> SubPropertyList { get; set; }
}