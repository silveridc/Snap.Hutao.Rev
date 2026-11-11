// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Primitive;

namespace Snap.Hutao.Rev.Web.Hutao.SpiralAbyss;

internal sealed class AvatarAppearanceRank
{
    public int Floor { get; set; }

    public List<ItemRate<AvatarId, double>> Ranks { get; set; } = default!;
}