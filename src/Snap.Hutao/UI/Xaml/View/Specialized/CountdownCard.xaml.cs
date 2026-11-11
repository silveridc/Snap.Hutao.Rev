// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml.Controls;
using Snap.Hutao.Rev.ViewModel.GachaLog;

namespace Snap.Hutao.Rev.UI.Xaml.View.Specialized;

[DependencyProperty<string>("Title")]
[DependencyProperty<IList<Countdown>>("Countdowns")]
internal sealed partial class CountdownCard : UserControl
{
    public CountdownCard()
    {
        InitializeComponent();
    }
}