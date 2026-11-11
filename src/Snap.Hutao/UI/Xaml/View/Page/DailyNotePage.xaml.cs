// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.UI.Xaml.Control;
using Snap.Hutao.Rev.ViewModel.DailyNote;

namespace Snap.Hutao.Rev.UI.Xaml.View.Page;

internal sealed partial class DailyNotePage : ScopedPage
{
    public DailyNotePage()
    {
        InitializeComponent();
    }

    protected override void LoadingOverride()
    {
        InitializeDataContext<DailyNoteViewModel>();
    }
}