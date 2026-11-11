// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.UI.Xaml.Control;
using Snap.Hutao.Rev.ViewModel.HutaoPassport;

namespace Snap.Hutao.Rev.UI.Xaml.View.Page;

internal sealed partial class HutaoPassportLoginLogPage : ScopedPage
{
    public HutaoPassportLoginLogPage()
    {
        InitializeComponent();
    }

    protected override void LoadingOverride()
    {
        InitializeDataContext<HutaoPassportLoginLogViewModel>();
    }
}
