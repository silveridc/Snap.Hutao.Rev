// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Service.Metadata.ContextAbstraction.ImmutableDictionary;

internal interface IMetadataDictionaryNameWeaponSource
{
    ImmutableDictionary<string, Model.Metadata.Weapon.Weapon> NameWeaponMap { get; set; }
}