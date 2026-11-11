// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Primitive;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Model.Metadata.Furniture;

internal sealed class FurnitureSuite
{
    public required FurnitureSuiteId Id { get; init; }

    public required ImmutableArray<FurnitureTypeId> Types { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }

    public required string ItemIcon { get; init; }

    public string? MapIcon { get; init; }

    public required ImmutableArray<AvatarId> FavoriteNpcs { get; init; }

    public required ImmutableArray<FurnitureId> Units { get; init; }
}