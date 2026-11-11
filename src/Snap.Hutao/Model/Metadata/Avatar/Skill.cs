// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Primitive;

namespace Snap.Hutao.Rev.Model.Metadata.Avatar;

internal class Skill
{
    public required SkillId Id { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }

    public required string Icon { get; init; }

    public ExtraLevelInfo? ExtraLevel { get; init; }
}