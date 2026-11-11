// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Entity;
using Snap.Hutao.Rev.ViewModel.GachaLog;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Service.GachaLog.Factory;

internal interface IGachaStatisticsFactory
{
    ValueTask<GachaStatistics> CreateAsync(GachaLogServiceMetadataContext context, ImmutableArray<GachaItem> items);
}