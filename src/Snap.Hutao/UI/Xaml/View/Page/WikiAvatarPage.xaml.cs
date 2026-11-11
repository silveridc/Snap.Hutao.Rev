// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.UI.Xaml.Control;
using Snap.Hutao.Rev.ViewModel.Wiki;

namespace Snap.Hutao.Rev.UI.Xaml.View.Page;

internal sealed partial class WikiAvatarPage : ScopedPage
{
    public WikiAvatarPage()
    {
        InitializeComponent();
    }

    protected override void LoadingOverride()
    {
        InitializeDataContext<WikiAvatarViewModel>();
    }
}