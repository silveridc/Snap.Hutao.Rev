// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Metadata;
using Snap.Hutao.Rev.Model.Primitive;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Service.Metadata.ContextAbstraction.ImmutableDictionary;

internal interface IMetadataDictionaryResultMaterialIdCombineSource
{
    ImmutableDictionary<MaterialId, Combine> ResultMaterialIdCombineMap { get; set; }
}