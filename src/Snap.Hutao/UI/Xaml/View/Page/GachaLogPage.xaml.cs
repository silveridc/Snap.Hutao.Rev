// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.UI.Xaml.Control;
using Snap.Hutao.Rev.ViewModel.GachaLog;

namespace Snap.Hutao.Rev.UI.Xaml.View.Page;

internal sealed partial class GachaLogPage : ScopedPage
{
    public GachaLogPage()
    {
        InitializeComponent();
    }

    protected override void LoadingOverride()
    {
        InitializeDataContext<GachaLogViewModel>();
    }
}