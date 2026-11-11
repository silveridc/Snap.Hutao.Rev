// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Win32.Foundation;

// ReSharper disable InconsistentNaming
[SuppressMessage("", "SA1307")]
internal struct POINT
{
    public int x;
    public int y;

    public POINT(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}