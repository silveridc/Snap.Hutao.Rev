// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.
using CommunityToolkit.WinUI.Behaviors;
using Microsoft.UI.Xaml.Controls;
using Snap.Hutao.Rev.Service.Navigation.Message;
using Snap.Hutao.Rev.UI.Content;

namespace Snap.Hutao.Rev.UI.Xaml.Behavior;

internal sealed class ServiceRecipientTitleBarBehavior : BehaviorBase<TitleBar>
{
    protected override void OnAssociatedObjectLoaded()
    {
        AssociatedObject.BackRequested += OnBackRequested;
        AssociatedObject.PaneToggleRequested += OnPaneToggleRequested;
    }

    protected override bool Uninitialize()
    {
        AssociatedObject.BackRequested -= OnBackRequested;
        AssociatedObject.PaneToggleRequested -= OnPaneToggleRequested;
        return base.Uninitialize();
    }

    private static void OnBackRequested(TitleBar sender, object args)
    {
        sender.XamlRoot.XamlContext()?.ServiceProvider.GetRequiredService<IMessenger>().Send(new NavigationGoBackMessage());
    }

    private static void OnPaneToggleRequested(TitleBar sender, object args)
    {
        sender.XamlRoot.XamlContext()?.ServiceProvider.GetRequiredService<IMessenger>().Send(new NavigationPaneToggleMessage());
    }
}