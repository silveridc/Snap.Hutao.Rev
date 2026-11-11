// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.Event.BbsSignReward;

internal sealed class Reward
{
    [JsonPropertyName("month")]
    public int Month { get; set; }

    [JsonPropertyName("awards")]
    public ImmutableArray<Award> Awards { get; set; } = default!;

    [JsonPropertyName("biz")]
    public string Biz { get; set; } = default!;

    [JsonPropertyName("resign")]
    public bool Resign { get; set; }

    [JsonPropertyName("short_extra_award")]
    public ShortExtraAward ShortExtraAward { get; set; } = default!;
}