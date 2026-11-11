// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Database;
using Snap.Hutao.Rev.Model.Entity;
using Snap.Hutao.Rev.Service.GachaLog.QueryProvider;
using Snap.Hutao.Rev.ViewModel.GachaLog;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Service.GachaLog;

internal interface IGachaLogService
{
    ValueTask<GachaArchive> EnsureArchiveInCollectionAsync(Guid archiveId, CancellationToken token = default);

    ValueTask<GachaStatistics> GetStatisticsAsync(GachaLogServiceMetadataContext context, GachaArchive archive);

    ValueTask<ImmutableArray<GachaStatisticsSlim>> GetStatisticsSlimImmutableArrayAsync(GachaLogServiceMetadataContext context, CancellationToken token = default);

    ValueTask<bool> RefreshGachaLogAsync(GachaLogServiceMetadataContext context, GachaLogQuery query, RefreshStrategyKind strategy, IProgress<GachaLogFetchStatus> progress, CancellationToken token);

    ValueTask RemoveArchiveAsync(GachaArchive archive);

    ValueTask<IAdvancedDbCollectionView<GachaArchive>> GetArchiveCollectionAsync();
}