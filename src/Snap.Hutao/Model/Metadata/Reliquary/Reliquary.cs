// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Intrinsic;
using Snap.Hutao.Rev.Model.Primitive;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Model.Metadata.Reliquary;

internal sealed class Reliquary
{
    public required ImmutableArray<ReliquaryId> Ids { get; init; }

    public required QualityType RankLevel { get; init; }

    public required ReliquarySetId SetId { get; init; }

    public required EquipType EquipType { get; init; }

    public required ItemType ItemType { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }

    public required string Icon { get; init; }
}