// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml;
using Microsoft.Web.WebView2.Core;
using Snap.Hutao.Rev.Service;
using Windows.Graphics;

namespace Snap.Hutao.Rev.UI.Xaml.View.Window.WebView2;

internal sealed class UpdateLogContentProvider : IWebView2ContentProvider
{
    private RemoteDocumentationContentProvider? provider;

    public ElementTheme ActualTheme { get; set; }

    public CoreWebView2? CoreWebView2 { get; set; }

    public Action? CloseWindowAction { get; set; }

    public ValueTask InitializeAsync(IServiceProvider serviceProvider, CancellationToken token)
    {
        string language = serviceProvider.GetRequiredService<CultureOptions>().LocaleName switch
        {
            LocaleNames.CHS or LocaleNames.CHT => "zh",
            LocaleNames.JP => "jp",
            _ => "en",
        };

        provider = new($"{language}/statements/latest.html")
        {
            ActualTheme = ActualTheme,
            CoreWebView2 = CoreWebView2,
            CloseWindowAction = CloseWindowAction,
        };
        return provider.InitializeAsync(serviceProvider, token);
    }

    public RectInt32 InitializePosition(RectInt32 parentRect, double parentDpi)
    {
        return WebView2WindowPosition.Vertical(parentRect, parentDpi);
    }

    public void Unload()
    {
        provider?.Unload();
    }
}
