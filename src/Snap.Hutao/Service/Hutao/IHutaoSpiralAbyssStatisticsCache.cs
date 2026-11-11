// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Primitive;
using Snap.Hutao.Rev.ViewModel.Complex;
using Snap.Hutao.Rev.Web.Hutao.SpiralAbyss;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Service.Hutao;

internal interface IHutaoSpiralAbyssStatisticsCache
{
    ImmutableArray<AvatarRankView> AvatarUsageRanks { get; set; }

    ImmutableArray<AvatarRankView> AvatarAppearanceRanks { get; set; }

    ImmutableArray<AvatarConstellationInfoView> AvatarConstellationInfos { get; set; }

    ImmutableArray<TeamAppearanceView> TeamAppearances { get; set; }

    Overview? Overview { get; set; }

    ImmutableDictionary<AvatarId, AvatarCollocationView>? AvatarCollocations { get; set; }

    ImmutableDictionary<WeaponId, WeaponCollocationView>? WeaponCollocations { get; set; }

    ValueTask InitializeForSpiralAbyssViewAsync(HutaoSpiralAbyssStatisticsMetadataContext context);

    ValueTask InitializeForWikiAvatarViewAsync(HutaoSpiralAbyssStatisticsMetadataContext context);

    ValueTask InitializeForWikiWeaponViewAsync(HutaoSpiralAbyssStatisticsMetadataContext context);
}