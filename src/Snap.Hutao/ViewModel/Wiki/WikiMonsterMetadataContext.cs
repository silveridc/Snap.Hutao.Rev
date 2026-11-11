// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Intrinsic;
using Snap.Hutao.Rev.Model.Metadata;
using Snap.Hutao.Rev.Model.Metadata.Item;
using Snap.Hutao.Rev.Model.Metadata.Monster;
using Snap.Hutao.Rev.Model.Primitive;
using Snap.Hutao.Rev.Service.Metadata.ContextAbstraction;
using Snap.Hutao.Rev.Service.Metadata.ContextAbstraction.ImmutableArray;
using Snap.Hutao.Rev.Service.Metadata.ContextAbstraction.ImmutableDictionary;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.ViewModel.Wiki;

internal sealed class WikiMonsterMetadataContext : IMetadataContext,
    IMetadataDictionaryLevelMonsterGrowCurveSource,
    IMetadataArrayMonsterSource,
    IMetadataDictionaryIdDisplayItemAndMaterialSource
{
    public ImmutableDictionary<Level, TypeValueCollection<GrowCurveType, float>> LevelDictionaryMonsterGrowCurveMap { get; set; } = default!;

    public ImmutableArray<Monster> Monsters { get; set; } = default!;

    public ImmutableDictionary<MaterialId, DisplayItem> IdDisplayItemAndMaterialMap { get; set; } = default!;
}