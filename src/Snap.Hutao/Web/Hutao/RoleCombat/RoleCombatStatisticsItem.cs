// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Primitive;
using Snap.Hutao.Rev.Web.Hutao.SpiralAbyss;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Web.Hutao.RoleCombat;

internal sealed class RoleCombatStatisticsItem
{
    public int RecordTotal { get; init; }

    public ImmutableArray<ItemRate<AvatarId, double>> BackupAvatarRates { get; init; }
}