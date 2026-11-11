// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Metadata.Avatar;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.ViewModel.Complex;

internal sealed class AvatarConstellationInfoView : AvatarView
{
    public AvatarConstellationInfoView(Avatar avatar, double rate, double? lastRate)
        : base(avatar, rate, lastRate)
    {
    }

    public required ImmutableArray<RateAndDelta> Rates { get; init; }
}