// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Intrinsic;
using Snap.Hutao.Rev.Model.Primitive;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Service.Metadata.ContextAbstraction.ImmutableDictionary;

internal interface IMetadataDictionaryIdReliquaryMainPropertySource
{
    ImmutableDictionary<ReliquaryMainAffixId, FightProperty> IdReliquaryMainPropertyMap { get; set; }
}