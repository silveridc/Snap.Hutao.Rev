// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.ViewModel.Achievement;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Service.Achievement;

internal interface IAchievementStatisticsService
{
    ValueTask<ImmutableArray<AchievementStatistics>> GetAchievementStatisticsAsync(AchievementServiceMetadataContext context, CancellationToken token = default);
}