// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.UI.Xaml.Control.AutoSuggestBox;

internal interface ITokenStringContainer
{
    string? Text { get; set; }

    bool IsLast { get; }
}