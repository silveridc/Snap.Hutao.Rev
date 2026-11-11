// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.Passport;

internal sealed class RiskVerify
{
    [JsonPropertyName("ticket")]
    public required string Ticket { get; set; }

    [JsonPropertyName("verify_type")]
    public required string VerifyType { get; set; }
}