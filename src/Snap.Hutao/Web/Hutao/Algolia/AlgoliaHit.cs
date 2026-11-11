// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hutao.Algolia;

internal sealed class AlgoliaHit
{
    [JsonPropertyName("url")]
    public string Url { get; set; } = default!;

    [JsonPropertyName("hierarchy")]
    public AlgoliaHierarchy Hierarchy { get; set; } = default!;
}