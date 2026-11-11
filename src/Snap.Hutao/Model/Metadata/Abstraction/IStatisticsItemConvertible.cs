// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.ViewModel.GachaLog;

namespace Snap.Hutao.Rev.Model.Metadata.Abstraction;

internal interface IStatisticsItemConvertible
{
    StatisticsItem ToStatisticsItem(int count);
}