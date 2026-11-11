// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.UI.Xaml.Control;
using Snap.Hutao.Rev.ViewModel.HardChallenge;

namespace Snap.Hutao.Rev.UI.Xaml.View.Page;

internal sealed partial class HardChallengePage : ScopedPage
{
    public HardChallengePage()
    {
        InitializeComponent();
    }

    protected override void LoadingOverride()
    {
        InitializeDataContext<HardChallengeViewModel>();
    }
}