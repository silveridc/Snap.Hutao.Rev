// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Intrinsic;
using Snap.Hutao.Rev.Model.Primitive;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Model.Metadata;

internal sealed class RoleCombatSchedule : IDefaultIdentity<RoleCombatScheduleId>
{
    public required RoleCombatScheduleId Id { get; init; }

    public required DateTimeOffset Begin { get; init; }

    public required DateTimeOffset End { get; init; }

    public required ImmutableArray<ElementType> Elements { get; init; }

    public required ImmutableArray<AvatarId> SpecialAvatars { get; init; }

    public required ImmutableArray<AvatarId> InitialAvatars { get; init; }
}