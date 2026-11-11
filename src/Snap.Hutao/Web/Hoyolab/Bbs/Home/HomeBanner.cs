// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.Bbs.Home;

internal sealed class HomeBanner
{
    [JsonPropertyName("cover")]
    public required Uri Cover { get; init; }

    [JsonPropertyName("app_path")]
    public required Uri AppPath { get; init; }
}