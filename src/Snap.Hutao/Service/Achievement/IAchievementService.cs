// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Database;
using Snap.Hutao.Rev.Model.InterChange.Achievement;
using Snap.Hutao.Rev.UI.Xaml.Data;
using Snap.Hutao.Rev.ViewModel.Achievement;
using System.Collections.Immutable;
using EntityArchive = Snap.Hutao.Rev.Model.Entity.AchievementArchive;

namespace Snap.Hutao.Rev.Service.Achievement;

internal interface IAchievementService
{
    ValueTask<IAdvancedDbCollectionView<EntityArchive>> GetArchiveCollectionAsync(CancellationToken token = default);

    ValueTask<UIAF> ExportToUIAFAsync(EntityArchive archive);

    IAdvancedCollectionView<AchievementView> GetAchievementViewCollection(EntityArchive archive, AchievementServiceMetadataContext context);

    ValueTask<ImportResult> ImportFromUIAFAsync(EntityArchive archive, ImmutableArray<UIAFItem> array, ImportStrategyKind strategy);

    ValueTask RemoveArchiveAsync(EntityArchive archive);

    void SaveAchievement(AchievementView achievement);

    ValueTask<ArchiveAddResultKind> AddArchiveAsync(EntityArchive newArchive);
}