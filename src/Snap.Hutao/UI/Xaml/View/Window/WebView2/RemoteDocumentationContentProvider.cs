// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml;
using Microsoft.Web.WebView2.Core;
using Windows.Graphics;
using Windows.System;

namespace Snap.Hutao.Rev.UI.Xaml.View.Window.WebView2;

internal sealed class RemoteDocumentationContentProvider : IWebView2ContentProvider
{
    private readonly string relativePath;

    public RemoteDocumentationContentProvider(string relativePath)
    {
        this.relativePath = relativePath.TrimStart('/');
    }

    public ElementTheme ActualTheme { get; set; }

    public CoreWebView2? CoreWebView2 { get; set; }

    public Action? CloseWindowAction { get; set; }

    public ValueTask InitializeAsync(IServiceProvider serviceProvider, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(CoreWebView2);

        CoreWebView2.NewWindowRequested += OnNewWindowRequested;
        CoreWebView2.Navigate($"{ServerHosts.Current.Doc}/{relativePath}");
        return ValueTask.CompletedTask;
    }

    public RectInt32 InitializePosition(RectInt32 parentRect, double parentDpi)
    {
        return WebView2WindowPosition.Vertical(parentRect, parentDpi);
    }

    public void Unload()
    {
        if (CoreWebView2 is not null)
        {
            CoreWebView2.NewWindowRequested -= OnNewWindowRequested;
        }
    }

    private static void OnNewWindowRequested(object? sender, CoreWebView2NewWindowRequestedEventArgs e)
    {
        e.Handled = true;
        _ = Launcher.LaunchUriAsync(e.Uri.ToUri());
    }
}
