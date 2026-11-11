// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.Event.BbsSignReward;

internal sealed class ExtraAward : Award
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("sign_day")]
    public int SignDay { get; set; }
}