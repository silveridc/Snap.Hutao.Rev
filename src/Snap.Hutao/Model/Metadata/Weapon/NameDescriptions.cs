// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Model.Metadata.Weapon;

internal sealed class NameDescriptions
{
    public required string Name { get; init; }

    public required ImmutableArray<LevelDescription> Descriptions { get; init; }
}