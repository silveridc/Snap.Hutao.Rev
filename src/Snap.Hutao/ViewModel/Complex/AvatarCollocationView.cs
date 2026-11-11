// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Primitive;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.ViewModel.Complex;

internal sealed class AvatarCollocationView
{
    public required AvatarId AvatarId { get; init; }

    public required ImmutableArray<AvatarView> Avatars { get; init; }

    public required ImmutableArray<WeaponView> Weapons { get; init; }

    public required ImmutableArray<ReliquarySetView> ReliquarySets { get; init; }
}