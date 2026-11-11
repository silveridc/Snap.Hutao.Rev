// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.UI.Windowing.Abstraction;

internal interface IXamlWindowClosedHandler
{
    void OnWindowClosing(out bool cancel);

    void OnWindowClosed();
}