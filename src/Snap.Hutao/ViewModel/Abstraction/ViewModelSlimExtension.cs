// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml;
using Snap.Hutao.Rev.UI.Xaml;
using WinRT;

namespace Snap.Hutao.Rev.ViewModel.Abstraction;

internal static class ViewModelSlimExtension
{
    extension(FrameworkElement frameworkElement)
    {
        public void InitializeViewModelSlim<TDataContext>(IServiceProvider serviceProvider)
            where TDataContext : ViewModelSlim
        {
            frameworkElement.Unloaded += OnFrameworkElementUnloaded;
            frameworkElement.InitializeDataContext<TDataContext>(serviceProvider);
        }
    }

    private static void OnFrameworkElementUnloaded(object sender, RoutedEventArgs e)
    {
        FrameworkElement frameworkElement = sender.As<FrameworkElement>();
        frameworkElement.Unloaded -= OnFrameworkElementUnloaded;
        frameworkElement.DataContext<ViewModelSlim>()?.IsViewUnloaded.Value = true;
    }
}