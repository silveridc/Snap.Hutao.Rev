// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Metadata;

namespace Snap.Hutao.Rev.ViewModel.GachaLog;

internal sealed class CountdownHistory
{
    public CountdownHistory(GachaEvent gachaEvent)
    {
        LastTime = gachaEvent.To;
        FormattedTime = $"{gachaEvent.To:yyyy-MM-dd}";
        FormattedVersionOrder = $"{gachaEvent.Version} {(gachaEvent.Order is 1 ? SH.ViewModelGachaLogCountdownOrderUp : SH.ViewModelGachaLogCountdownOrderDown)}";
        Banner = gachaEvent.Banner;
    }

    public string FormattedTime { get; }

    public string FormattedVersionOrder { get; }

    public Uri Banner { get; }

    internal DateTimeOffset LastTime { get; }
}