// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model;
using Snap.Hutao.Rev.Model.Metadata;
using Snap.Hutao.Rev.Model.Metadata.Abstraction;
using Snap.Hutao.Rev.Model.Metadata.Item;
using Snap.Hutao.Rev.Model.Primitive;
using Snap.Hutao.Rev.Service.Metadata.ContextAbstraction;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Service.Cultivation;

internal class CultivationMetadataContext : ICultivationMetadataContext
{
    public ImmutableArray<Material> Materials { get; set; } = default!;

    public ImmutableDictionary<MaterialId, Material> IdMaterialMap { get; set; } = default!;

    public ImmutableDictionary<AvatarId, Model.Metadata.Avatar.Avatar> IdAvatarMap { get; set; } = default!;

    public ImmutableDictionary<WeaponId, Model.Metadata.Weapon.Weapon> IdWeaponMap { get; set; } = default!;

    public ImmutableDictionary<MaterialId, Combine> ResultMaterialIdCombineMap { get; set; } = default!;

    public Item GetAvatarItem(AvatarId avatarId)
    {
        return this.GetAvatar(avatarId).GetOrCreateItem();
    }

    public Item GetWeaponItem(WeaponId weaponId)
    {
        return this.GetWeapon(weaponId).GetOrCreateItem();
    }
}