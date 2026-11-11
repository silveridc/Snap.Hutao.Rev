// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.UI.Xaml.Control;
using Snap.Hutao.Rev.ViewModel.Setting;

namespace Snap.Hutao.Rev.UI.Xaml.View.Page;

internal sealed partial class SettingPage : ScopedPage
{
    public SettingPage()
    {
        InitializeComponent();
    }

    protected override void LoadingOverride()
    {
        InitializeDataContext<SettingViewModel>();
        this.DataContext<SettingViewModel>()?.AttachXamlElement(RootScrollViewer, GachaLogBorder);
    }
}