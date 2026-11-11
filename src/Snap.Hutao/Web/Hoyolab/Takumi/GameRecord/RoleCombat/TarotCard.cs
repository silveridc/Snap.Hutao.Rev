// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.RoleCombat;

internal sealed class TarotCard
{
    [JsonPropertyName("icon")]
    public required Uri Icon { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("is_unlock")]
    public required bool IsUnlock { get; init; }

    [JsonPropertyName("unlock_num")]
    public required int UnlockNumber { get; init; }
}