// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model;

namespace Snap.Hutao.Rev.ViewModel.GachaLog;

internal sealed class SummaryItem : Item
{
    public bool IsUp { get; set; }

    public bool IsGuarantee { get; set; }

    // Used in ListView for ProgressBar Maximum Value
    public int GuaranteeOrangeThreshold { get; set; }

    public int LastPull { get; set; }

    public string FormattedTime
    {
        get => $"{Time.ToLocalTime():yyy.MM.dd HH:mm:ss}";
    }

    public Windows.UI.Color Color { get; set; }

    internal DateTimeOffset Time { get; set; }
}