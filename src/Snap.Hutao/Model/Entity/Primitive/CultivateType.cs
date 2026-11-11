// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Model.Entity.Primitive;

[ExtendedEnum]
internal enum CultivateType
{
    None,

    [LocalizationKey(nameof(SH.ModelEntityPrimitiveCultivateTypeAvatarAndSkill))]
    AvatarAndSkill,

    [LocalizationKey(nameof(SH.ModelEntityPrimitiveCultivateTypeWeapon))]
    Weapon,

    [LocalizationKey(nameof(SH.ModelEntityPrimitiveCultivateTypeFurniture))]
    Furniture,
}