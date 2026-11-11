// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml.Controls;
using Snap.Hutao.Rev.Service.GachaLog;

namespace Snap.Hutao.Rev.UI.Xaml.View.Dialog;

[DependencyProperty<GachaLogFetchStatus>("Status")]
internal sealed partial class GachaLogRefreshProgressDialog : ContentDialog
{
    public GachaLogRefreshProgressDialog()
    {
        InitializeComponent();
    }

    public void OnReport(GachaLogFetchStatus status)
    {
        Status = status;
    }
}