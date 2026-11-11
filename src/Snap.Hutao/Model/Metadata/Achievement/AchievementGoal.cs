// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Primitive;

namespace Snap.Hutao.Rev.Model.Metadata.Achievement;

internal sealed class AchievementGoal
{
    public required AchievementGoalId Id { get; init; }

    public required uint Order { get; init; }

    public required string Name { get; init; }

    public Reward? FinishReward { get; init; }

    public required string Icon { get; init; }
}