// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model;
using Snap.Hutao.Rev.Model.Intrinsic;

namespace Snap.Hutao.Rev.ViewModel.Complex;

internal abstract class CollocationView : RateAndDelta, INameIcon<Uri>
{
    protected CollocationView(double rate, double? lastRate)
        : base(rate, lastRate)
    {
    }

    public abstract string Name { get; }

    public abstract Uri Icon { get; }

    public abstract QualityType Quality { get; }
}