// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hutao.SpiralAbyss;

internal class ItemRate<TItem, TRate>
{
    public ItemRate(TItem item, TRate rate)
    {
        Item = item;
        Rate = rate;
    }

    public TItem Item { get; set; }

    public TRate Rate { get; set; }
}