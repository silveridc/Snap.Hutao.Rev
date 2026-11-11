// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.Passport;

internal sealed class ReactivateInfo
{
    [JsonPropertyName("required")]
    public bool Required { get; set; }

    [JsonPropertyName("ticket")]
    public string Ticket { get; set; } = default!;
}