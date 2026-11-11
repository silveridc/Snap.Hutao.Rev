// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.Bbs.Home;

internal sealed class LiveDataAward
{
    [JsonPropertyName("icon")]
    public required Uri Icon { get; init; }

    [JsonPropertyName("desc")]
    public required string Description { get; init; }
}