// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Primitive;
using Snap.Hutao.Rev.Service.Metadata.ContextAbstraction;
using Snap.Hutao.Rev.Service.Metadata.ContextAbstraction.ImmutableArray;
using Snap.Hutao.Rev.Service.Metadata.ContextAbstraction.ImmutableDictionary;
using System.Collections.Immutable;
using MetadataAchievement = Snap.Hutao.Rev.Model.Metadata.Achievement.Achievement;

namespace Snap.Hutao.Rev.Service.Achievement;

internal sealed class AchievementServiceMetadataContext : IMetadataContext,
    IMetadataArrayAchievementSource,
    IMetadataDictionaryIdAchievementSource
{
    public ImmutableArray<MetadataAchievement> Achievements { get; set; } = default!;

    public ImmutableDictionary<AchievementId, MetadataAchievement> IdAchievementMap { get; set; } = default!;
}