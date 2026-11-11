// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Intrinsic;
using Snap.Hutao.Rev.ViewModel.GachaLog;

namespace Snap.Hutao.Rev.Model.Metadata.Abstraction;

internal interface ISummaryItemConvertible
{
    QualityType Quality { get; }

    SummaryItem ToSummaryItem(int lastPull, in DateTimeOffset time, bool isUp);
}