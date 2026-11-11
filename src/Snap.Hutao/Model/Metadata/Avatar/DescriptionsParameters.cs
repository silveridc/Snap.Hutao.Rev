// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Primitive;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Model.Metadata.Avatar;

internal sealed class DescriptionsParameters
{
    public required ImmutableArray<string> Descriptions { get; init; }

    public required IdLevelParametersCollection<ProudSkillId, SkillLevel, float> Parameters { get; init; }
}