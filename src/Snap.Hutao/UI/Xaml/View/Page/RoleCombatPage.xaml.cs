// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.UI.Xaml.Control;
using Snap.Hutao.Rev.ViewModel.RoleCombat;

namespace Snap.Hutao.Rev.UI.Xaml.View.Page;

internal sealed partial class RoleCombatPage : ScopedPage
{
    public RoleCombatPage()
    {
        InitializeComponent();
    }

    protected override void LoadingOverride()
    {
        InitializeDataContext<RoleCombatViewModel>();
    }
}