// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Intrinsic;
using Snap.Hutao.Rev.Model.Metadata.Converter;
using Snap.Hutao.Rev.Model.Metadata.Weapon;

namespace Snap.Hutao.Rev.ViewModel.Complex;

internal sealed class WeaponView : CollocationView
{
    public WeaponView(Weapon weapon, double rate, double? lastRate)
        : base(rate, lastRate)
    {
        Name = weapon.Name;
        Icon = EquipIconConverter.IconNameToUri(weapon.Icon);
        Quality = weapon.Quality;
    }

    public override string Name { get; }

    public override Uri Icon { get; }

    public override QualityType Quality { get; }
}