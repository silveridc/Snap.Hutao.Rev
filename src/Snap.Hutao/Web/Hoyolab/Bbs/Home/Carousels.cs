// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Web.Hoyolab.Bbs.Home;

internal sealed class Carousels
{
    [JsonPropertyName("position")]
    public required int Position { get; init; }

    [JsonPropertyName("data")]
    public required ImmutableArray<HomeBanner> Data { get; init; }
}