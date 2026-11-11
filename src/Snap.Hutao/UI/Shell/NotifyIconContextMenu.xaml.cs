// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml.Controls;
using Snap.Hutao.Rev.UI.Xaml;
using Snap.Hutao.Rev.ViewModel;

namespace Snap.Hutao.Rev.UI.Shell;

internal sealed partial class NotifyIconContextMenu : Flyout
{
    public NotifyIconContextMenu(IServiceProvider serviceProvider)
    {
        AllowFocusOnInteraction = false;
        InitializeComponent();
        Root.InitializeDataContext<NotifyIconViewModel>(serviceProvider);
    }
}