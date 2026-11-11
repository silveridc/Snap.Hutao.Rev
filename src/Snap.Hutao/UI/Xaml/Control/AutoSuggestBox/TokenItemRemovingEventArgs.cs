// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using CommunityToolkit.Common.Deferred;

namespace Snap.Hutao.Rev.UI.Xaml.Control.AutoSuggestBox;

internal sealed class TokenItemRemovingEventArgs : DeferredCancelEventArgs
{
    public TokenItemRemovingEventArgs(object item, AutoSuggestTokenBoxItem token)
    {
        Item = item;
        Token = token;
    }

    public object Item { get; private set; }

    public AutoSuggestTokenBoxItem Token { get; private set; }
}