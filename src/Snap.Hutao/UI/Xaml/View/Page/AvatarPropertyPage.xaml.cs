// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.UI.Xaml.Control;
using Snap.Hutao.Rev.ViewModel.AvatarProperty;

namespace Snap.Hutao.Rev.UI.Xaml.View.Page;

internal sealed partial class AvatarPropertyPage : ScopedPage
{
    public AvatarPropertyPage()
    {
        InitializeComponent();
    }

    protected override void LoadingOverride()
    {
        InitializeDataContext<AvatarPropertyViewModel>();
    }
}