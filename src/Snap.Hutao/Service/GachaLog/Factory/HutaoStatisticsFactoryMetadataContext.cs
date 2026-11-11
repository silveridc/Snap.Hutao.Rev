// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Metadata;
using Snap.Hutao.Rev.Model.Metadata.Avatar;
using Snap.Hutao.Rev.Model.Metadata.Weapon;
using Snap.Hutao.Rev.Model.Primitive;
using Snap.Hutao.Rev.Service.Metadata.ContextAbstraction;
using Snap.Hutao.Rev.Service.Metadata.ContextAbstraction.ImmutableArray;
using Snap.Hutao.Rev.Service.Metadata.ContextAbstraction.ImmutableDictionary;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Service.GachaLog.Factory;

internal sealed class HutaoStatisticsFactoryMetadataContext : IMetadataContext,
    IMetadataDictionaryIdAvatarSource,
    IMetadataDictionaryIdWeaponSource,
    IMetadataArrayGachaEventSource
{
    public ImmutableDictionary<AvatarId, Avatar> IdAvatarMap { get; set; } = default!;

    public ImmutableDictionary<WeaponId, Weapon> IdWeaponMap { get; set; } = default!;

    public ImmutableArray<GachaEvent> GachaEvents { get; set; } = default!;
}