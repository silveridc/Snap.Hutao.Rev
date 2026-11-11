// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Web.WebView2.Core;
using Snap.Hutao.Rev.ViewModel.User;
using Snap.Hutao.Rev.Web.Bridge;

namespace Snap.Hutao.Rev.UI.Xaml.View.Window.WebView2;

internal interface IJSBridgeUriSourceProvider
{
    MiHoYoJSBridge CreateJSBridge(IServiceProvider serviceProvider, CoreWebView2 coreWebView2, UserAndUid userAndUid);

    string? GetSource(UserAndUid userAndUid);
}