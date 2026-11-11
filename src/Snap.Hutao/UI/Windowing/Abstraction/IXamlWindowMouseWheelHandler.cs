// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.UI.Input;

namespace Snap.Hutao.Rev.UI.Windowing.Abstraction;

internal interface IXamlWindowMouseWheelHandler
{
    void OnMouseWheel(ref readonly PointerPointProperties data);
}