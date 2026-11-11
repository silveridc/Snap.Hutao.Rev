// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.UI.Xaml.Control;
using Snap.Hutao.Rev.ViewModel.Feedback;

namespace Snap.Hutao.Rev.UI.Xaml.View.Page;

internal sealed partial class FeedbackPage : ScopedPage
{
    public FeedbackPage()
    {
        InitializeComponent();
    }

    protected override void LoadingOverride()
    {
        InitializeDataContext<FeedbackViewModel>();
    }
}