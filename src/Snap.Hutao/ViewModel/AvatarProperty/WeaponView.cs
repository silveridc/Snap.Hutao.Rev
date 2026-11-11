// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model;
using Snap.Hutao.Rev.Model.Calculable;
using Snap.Hutao.Rev.Model.Intrinsic;
using Snap.Hutao.Rev.Model.Primitive;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.ViewModel.AvatarProperty;

internal sealed class WeaponView : EquipView, ICalculableSource<ICalculableWeapon>
{
    public NameValue<string>? SubProperty { get; set; }

    public uint AffixLevelNumber { get; set; }

    public string AffixLevel { get => SH.FormatModelBindingAvatarPropertyWeaponAffix(AffixLevelNumber); }

    public string AffixName { get; set; } = default!;

    public string AffixDescription { get; set; } = default!;

    public ImmutableArray<bool> PromoteArray { get; set; }

    internal WeaponId Id { get; set; }

    internal uint LevelNumber { get; set; }

    internal uint MaxLevel { get => Model.Metadata.Weapon.Weapon.GetMaxLevelByQuality(Quality); }

    internal WeaponType WeaponType { get; set; }

    internal PromoteLevel PromoteLevel { get; set; }

    public ICalculableWeapon ToCalculable()
    {
        return CalculableWeapon.From(this);
    }
}