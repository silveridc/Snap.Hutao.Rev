// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.Avatar;

internal sealed class RecommendRelicProperty
{
    [JsonPropertyName("recommend_properties")]
    public RecommendProperties RecommendProperties { get; set; } = default!;

    // Ignored field custom_properties
    // Ignored field has_set_recommend_prop
}