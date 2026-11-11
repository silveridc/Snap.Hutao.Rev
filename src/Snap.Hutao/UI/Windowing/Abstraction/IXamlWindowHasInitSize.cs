// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Windows.Graphics;

namespace Snap.Hutao.Rev.UI.Windowing.Abstraction;

internal interface IXamlWindowHasInitSize
{
    SizeInt32 InitSize { get; }
}