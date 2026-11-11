// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Model.Metadata.Tower;

internal sealed class TowerWave
{
    public required WaveType Type { get; init; }

    public string? Description { get; init; }

    public required ImmutableArray<TowerMonster> Monsters { get; init; }
}