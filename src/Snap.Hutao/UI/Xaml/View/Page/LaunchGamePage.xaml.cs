// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Snap.Hutao.Rev.UI.Xaml.Control;
using Snap.Hutao.Rev.ViewModel.Game;

namespace Snap.Hutao.Rev.UI.Xaml.View.Page;

internal sealed partial class LaunchGamePage : ScopedPage
{
    public LaunchGamePage()
    {
        InitializeComponent();
    }

    protected override void LoadingOverride()
    {
        InitializeDataContext<LaunchGameViewModel>();
    }

    private void OnFpsUnlockDisabledClick(object sender, RoutedEventArgs e)
    {
        ShowFeatureDisabledTip(FpsUnlockDisabledTeachingTip, (DataContext as LaunchGameViewModel)?.LaunchOptions.FpsUnlockDisabledReason.Value);
    }

    private void OnIslandDisabledClick(object sender, RoutedEventArgs e)
    {
        ShowFeatureDisabledTip(IslandDisabledTeachingTip, (DataContext as LaunchGameViewModel)?.LaunchOptions.IslandDisabledReason.Value);
    }

    private static void ShowFeatureDisabledTip(TeachingTip teachingTip, string? subtitle)
    {
        teachingTip.Subtitle = subtitle ?? SH.ViewPageLaunchGameFeatureDisabledDefaultReason;
        teachingTip.IsOpen = true;
    }
}
