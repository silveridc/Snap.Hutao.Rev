// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Immutable;

namespace Snap.Hutao.Rev.ViewModel.GachaLog;

internal sealed class WishCountdownBundle
{
    public required ImmutableArray<Countdown> OrangeAvatars { get; init; }

    public required ImmutableArray<Countdown> PurpleAvatars { get; init; }

    public required ImmutableArray<Countdown> OrangeWeapons { get; init; }

    public required ImmutableArray<Countdown> PurpleWeapons { get; init; }
}