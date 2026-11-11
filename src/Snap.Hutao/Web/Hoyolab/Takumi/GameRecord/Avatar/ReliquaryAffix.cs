// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.Avatar;

internal sealed class ReliquaryAffix
{
    [JsonPropertyName("activation_number")]
    public int ActivationNumber { get; set; }

    [JsonPropertyName("effect")]
    public string Effect { get; set; } = default!;
}