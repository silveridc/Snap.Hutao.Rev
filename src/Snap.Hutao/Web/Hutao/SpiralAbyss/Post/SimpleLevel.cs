// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.SpiralAbyss;

namespace Snap.Hutao.Rev.Web.Hutao.SpiralAbyss.Post;

internal sealed class SimpleLevel
{
    public SimpleLevel(SpiralAbyssLevel level)
    {
        Index = level.Index;
        Star = level.Star;
        Battles = level.Battles.Select(b => new SimpleBattle(b));
    }

    public uint Index { get; set; }

    public int Star { get; set; }

    public IEnumerable<SimpleBattle> Battles { get; set; }
}