// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Bridge.Model;

internal sealed class CookieTokenPayload
{
    [JsonPropertyName("forceRefresh")]
    public bool ForceRefresh { get; set; }
}