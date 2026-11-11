// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Primitive;

namespace Snap.Hutao.Rev.Web.Hutao.SpiralAbyss;

internal sealed class WeaponCollocation : WeaponBuild
{
    public List<ItemRate<AvatarId, double>> Avatars { get; set; } = default!;
}