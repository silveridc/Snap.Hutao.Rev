// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Web.Response;

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.RoleCombat;

internal sealed class TarotCardState : ListWrapper<TarotCard>
{
    [JsonPropertyName("total_num")]
    public required int TotalNumber { get; init; }

    [JsonPropertyName("curr_num")]
    public required int CurrentNumber { get; init; }
}