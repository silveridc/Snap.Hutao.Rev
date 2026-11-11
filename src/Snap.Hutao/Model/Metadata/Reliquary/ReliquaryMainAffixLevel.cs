// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Intrinsic;

namespace Snap.Hutao.Rev.Model.Metadata.Reliquary;

internal sealed class ReliquaryMainAffixLevel
{
    public required QualityType Rank { get; init; }

    public required uint Level { get; init; }

    public required TypeValueCollection<FightProperty, float> Properties { get; init; }
}