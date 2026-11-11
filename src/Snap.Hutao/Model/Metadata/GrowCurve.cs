// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Intrinsic;
using Snap.Hutao.Rev.Model.Primitive;

namespace Snap.Hutao.Rev.Model.Metadata;

internal sealed class GrowCurve
{
    public required Level Level { get; init; }

    public required TypeValueCollection<GrowCurveType, float> Curves { get; init; }
}