// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Web.WebView2.Core;
using Snap.Hutao.Rev.UI.Xaml.View.Window.WebView2;
using Snap.Hutao.Rev.ViewModel.User;
using Snap.Hutao.Rev.Web.Bridge;
using Snap.Hutao.Rev.Web.Hoyolab;

namespace Snap.Hutao.Rev.ViewModel.DailyNote;

internal sealed class DailyJSBridgeUriSourceProvider : IJSBridgeUriSourceProvider
{
    public MiHoYoJSBridge CreateJSBridge(IServiceProvider serviceProvider, CoreWebView2 coreWebView2, UserAndUid userAndUid)
    {
        return ActivatorUtilities.CreateInstance<MiHoYoJSBridge>(serviceProvider, coreWebView2, userAndUid);
    }

    public string GetSource(UserAndUid userAndUid)
    {
        string query = userAndUid.Uid.ToRoleIdServerQueryString();
        return $"https://webstatic.mihoyo.com/app/community-game-records/index.html?bbs_presentation_style=fullscreen#/ys/daily/?{query}";
    }
}