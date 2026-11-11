// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Graphics;
using Snap.Hutao.Rev.UI.Windowing;
using Windows.Graphics;

namespace Snap.Hutao.Rev.UI.Xaml.View.Window;

internal static class ScaledSizeInt32
{
    public static SizeInt32 CreateForWindow(int width, int height, Microsoft.UI.Xaml.Window window)
    {
        return new SizeInt32(width, height).Scale(window.RasterizationScale);
    }
}