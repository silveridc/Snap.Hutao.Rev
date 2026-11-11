// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.HoyoPlay.Connect.Branch;

internal sealed class Category
{
    [JsonPropertyName("category_id")]
    public string CategoryId { get; set; } = default!;

    [JsonPropertyName("matching_field")]
    public string MatchingField { get; set; } = default!;
}