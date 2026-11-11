// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.Passport;

internal sealed class LTokenWrapper
{
    [JsonPropertyName("ltoken")]
    public string LToken { get; set; } = default!;
}