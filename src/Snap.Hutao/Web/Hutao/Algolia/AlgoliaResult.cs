// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hutao.Algolia;

internal sealed class AlgoliaResult
{
    [JsonPropertyName("hits")]
    public List<AlgoliaHit> Hits { get; set; } = default!;
}