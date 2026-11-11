// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Intrinsic;
using Snap.Hutao.Rev.Model.Primitive;

namespace Snap.Hutao.Rev.Model.Metadata.Item;

internal class DisplayItem : IDefaultIdentity<MaterialId>
{
    public required MaterialId Id { get; init; }

    public required QualityType RankLevel { get; init; }

    public required ItemType ItemType { get; init; }

    public required string Icon { get; init; }

    public required string Name { get; init; }

    public uint Rank { get; init; }

    public string? Description { get; init; }

    public string? TypeDescription { get; init; }
}