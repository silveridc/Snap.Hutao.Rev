// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Model.Calculable;

internal readonly struct CalculableOptions
{
    public readonly ICalculableAvatar? Avatar;

    public readonly ICalculableWeapon? Weapon;

    public CalculableOptions(ICalculableAvatar? avatar, ICalculableWeapon? weapon)
    {
        Avatar = avatar;
        Weapon = weapon;
    }

    public CalculableOptions(ICalculableSource<ICalculableAvatar>? avatar, ICalculableSource<ICalculableWeapon>? weapon)
        : this(avatar?.ToCalculable(), weapon?.ToCalculable())
    {
    }
}