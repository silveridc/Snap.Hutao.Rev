// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Intrinsic;
using Snap.Hutao.Rev.Model.Primitive;

namespace Snap.Hutao.Rev.Model.Metadata;

internal sealed class Promote
{
    public required PromoteId Id { get; init; }

    public required PromoteLevel Level { get; init; }

    public required TypeValueCollection<FightProperty, float> AddProperties { get; init; }
}