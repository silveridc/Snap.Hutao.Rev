// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model;
using Snap.Hutao.Rev.Model.Primitive;
using Snap.Hutao.Rev.Service.Metadata.ContextAbstraction;
using Snap.Hutao.Rev.Service.Metadata.ContextAbstraction.ImmutableArray;
using Snap.Hutao.Rev.Service.Metadata.ContextAbstraction.ImmutableDictionary;

namespace Snap.Hutao.Rev.Service.Cultivation;

internal interface ICultivationMetadataContext : IMetadataContext,
    IMetadataArrayMaterialSource,
    IMetadataDictionaryIdMaterialSource,
    IMetadataDictionaryIdAvatarSource,
    IMetadataDictionaryIdWeaponSource,
    IMetadataDictionaryResultMaterialIdCombineSource
{
    Item GetAvatarItem(AvatarId avatarId);

    Item GetWeaponItem(WeaponId weaponId);
}