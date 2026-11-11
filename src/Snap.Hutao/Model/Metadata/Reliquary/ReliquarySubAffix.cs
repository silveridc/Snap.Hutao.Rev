// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Intrinsic;
using Snap.Hutao.Rev.Model.Primitive;

namespace Snap.Hutao.Rev.Model.Metadata.Reliquary;

internal sealed class ReliquarySubAffix : IDefaultIdentity<ReliquarySubAffixId>
{
    public required ReliquarySubAffixId Id { get; init; }

    public required FightProperty Type { get; init; }

    public required float Value { get; init; }
}