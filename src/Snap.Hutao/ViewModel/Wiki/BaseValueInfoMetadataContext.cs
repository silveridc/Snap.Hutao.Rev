// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.
using Snap.Hutao.Rev.Model.Intrinsic;
using Snap.Hutao.Rev.Model.Metadata;
using Snap.Hutao.Rev.Model.Primitive;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.ViewModel.Wiki;

internal sealed class BaseValueInfoMetadataContext
{
    public required ImmutableDictionary<Level, TypeValueCollection<GrowCurveType, float>> GrowCurveMap { get; set; }

    public required ImmutableDictionary<PromoteLevel, Promote>? PromoteMap { get; set; }
}