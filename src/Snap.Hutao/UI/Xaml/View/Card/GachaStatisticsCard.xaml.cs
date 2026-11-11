// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml.Controls;
using Snap.Hutao.Rev.ViewModel.Abstraction;

namespace Snap.Hutao.Rev.UI.Xaml.View.Card;

internal sealed partial class GachaStatisticsCard : Button
{
    public GachaStatisticsCard(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        this.InitializeViewModelSlim<ViewModel.GachaLog.GachaLogViewModelSlim>(serviceProvider);
    }
}