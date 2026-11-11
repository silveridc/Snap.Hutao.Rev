// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Primitive;
using Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.SpiralAbyss;

namespace Snap.Hutao.Rev.Web.Hutao.SpiralAbyss.Post;

internal sealed class SimpleBattle
{
    public SimpleBattle(SpiralAbyssBattle battle)
    {
        Index = battle.Index;
        Avatars = battle.Avatars.Select(a => a.Id);
    }

    public int Index { get; set; }

    public IEnumerable<AvatarId> Avatars { get; set; }
}