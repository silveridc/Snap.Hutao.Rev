// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.ViewModel.Cultivation;

namespace Snap.Hutao.Rev.Service.Cultivation;

internal interface ICultivationResinStatisticsService
{
    ValueTask<ResinStatistics> GetResinStatisticsAsync(IEnumerable<StatisticsCultivateItem> statisticsCultivateItems, CancellationToken token);
}