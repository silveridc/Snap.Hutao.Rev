// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.RoleCombat;

internal sealed class RoleCombatSplendourBuffWrapper
{
    [JsonPropertyName("summary")]
    public required RoleCombatSplendourBuffSummary Summary { get; init; }

    [JsonPropertyName("buffs")]
    public required ImmutableArray<RoleCombatSplendourBuff> Buffs { get; init; }
}