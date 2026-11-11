// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.UI.Xaml.Control;
using Snap.Hutao.Rev.ViewModel.SpiralAbyss;

namespace Snap.Hutao.Rev.UI.Xaml.View.Page;

internal sealed partial class SpiralAbyssRecordPage : ScopedPage
{
    public SpiralAbyssRecordPage()
    {
        InitializeComponent();
    }

    protected override void LoadingOverride()
    {
        InitializeDataContext<SpiralAbyssViewModel>();
    }
}