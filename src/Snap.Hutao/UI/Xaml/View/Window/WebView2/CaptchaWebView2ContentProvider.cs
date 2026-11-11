// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml;
using Microsoft.Web.WebView2.Core;
using Snap.Hutao.Rev.Core.Graphics;
using Snap.Hutao.Rev.Web.WebView2;
using System.Collections.Specialized;
using System.Web;
using Windows.Graphics;

namespace Snap.Hutao.Rev.UI.Xaml.View.Window.WebView2;

internal sealed class CaptchaWebView2ContentProvider : IWebView2ContentProvider
{
    private const string CallbackUrlPrefix = "https://captcha.snaphutao.invalid/callback";

    private readonly string pageUrl;
    private readonly TaskCompletionSource<string?> tokenTcs = new();

    public CaptchaWebView2ContentProvider(string pageUrl)
    {
        this.pageUrl = pageUrl;
    }

    public ElementTheme ActualTheme { get; set; }

    public CoreWebView2? CoreWebView2 { get; set; }

    public Action? CloseWindowAction { get; set; }

    public ValueTask InitializeAsync(IServiceProvider serviceProvider, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(CoreWebView2);

        CoreWebView2.DisableAutoCompletion();
        CoreWebView2.NavigationStarting += OnNavigationStarting;
        CoreWebView2.Navigate(pageUrl);

        return ValueTask.CompletedTask;
    }

    public RectInt32 InitializePosition(RectInt32 parentRect, double parentDpi)
    {
        SizeInt32 size = new SizeInt32(420, 260).Scale(parentDpi);
        return new RectInt32(parentRect.X + ((parentRect.Width - size.Width) / 2), parentRect.Y + ((parentRect.Height - size.Height) / 2), size.Width, size.Height);
    }

    public void Unload()
    {
        if (CoreWebView2 is not null)
        {
            CoreWebView2.NavigationStarting -= OnNavigationStarting;
        }

        tokenTcs.TrySetResult(null);
    }

    public async ValueTask<string?> GetTokenAsync()
    {
        return await tokenTcs.Task.ConfigureAwait(false);
    }

    private void OnNavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
    {
        if (!e.Uri.StartsWith(CallbackUrlPrefix, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        // 哨兵地址只是回传令牌的载体，不会真的访问
        e.Cancel = true;

        NameValueCollection query = HttpUtility.ParseQueryString(new Uri(e.Uri).Query);
        string? token = query["token"];
        tokenTcs.TrySetResult(string.IsNullOrEmpty(token) ? null : token);
        CloseWindowAction?.Invoke();
    }
}
