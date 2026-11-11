// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hutao.Algolia;

internal sealed class AlgoliaRequestsWrapper
{
    [JsonPropertyName("requests")]
    public List<AlgoliaRequest> Requests { get; set; } = default!;
}