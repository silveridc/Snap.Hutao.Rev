// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Extension;

internal static class SizeExtension
{
    public static double Size(this Windows.Foundation.Size size)
    {
        return size.Width * size.Height;
    }
}