// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml;
using Snap.Hutao.Rev.UI.Windowing;
using Snap.Hutao.Rev.UI.Windowing.Abstraction;
using Snap.Hutao.Rev.ViewModel.Scripting;
using System.Collections.Immutable;
using Windows.Graphics;

namespace Snap.Hutao.Rev.UI.Xaml.View.Window;

[Service(ServiceLifetime.Transient)]
internal sealed partial class ScriptingWindow : Microsoft.UI.Xaml.Window, IXamlWindowExtendContentIntoTitleBar, IXamlWindowHasInitSize
{
    public ScriptingWindow(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        IServiceScope scope = serviceProvider.CreateScope();
        RootGrid.InitializeDataContext<ScriptingViewModel>(scope.ServiceProvider);
        this.InitializeController(scope.ServiceProvider);
    }

    public FrameworkElement TitleBarCaptionAccess { get => DragableGrid; }

    public ImmutableArray<FrameworkElement> TitleBarPassthrough { get; } = [];

    public SizeInt32 InitSize { get => ScaledSizeInt32.CreateForWindow(800, 500, this); }
}