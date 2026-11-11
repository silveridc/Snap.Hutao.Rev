// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.ViewModel.Complex;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Service.Hutao;

internal interface IHutaoRoleCombatStatisticsCache
{
    int RecordTotal { get; }

    ImmutableArray<AvatarView> AvatarAppearances { get; }

    ValueTask InitializeForRoleCombatViewAsync(HutaoRoleCombatStatisticsMetadataContext context);
}