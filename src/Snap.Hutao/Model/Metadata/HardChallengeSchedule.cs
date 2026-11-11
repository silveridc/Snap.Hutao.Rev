// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Primitive;

namespace Snap.Hutao.Rev.Model.Metadata;

internal sealed class HardChallengeSchedule : IDefaultIdentity<HardChallengeScheduleId>
{
    public required HardChallengeScheduleId Id { get; init; }

    public required DateTimeOffset Begin { get; init; }

    public required DateTimeOffset End { get; init; }

    public required string Name { get; init; }
}