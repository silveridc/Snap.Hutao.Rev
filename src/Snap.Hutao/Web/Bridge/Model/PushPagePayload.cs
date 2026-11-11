// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Bridge.Model;

internal sealed class PushPagePayload
{
    [JsonPropertyName("page")]
    public string Page { get; set; } = default!;
}