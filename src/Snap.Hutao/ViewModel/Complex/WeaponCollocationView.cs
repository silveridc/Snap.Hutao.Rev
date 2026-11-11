// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Primitive;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.ViewModel.Complex;

internal sealed class WeaponCollocationView
{
    public required ImmutableArray<AvatarView> Avatars { get; init; }

    internal required WeaponId WeaponId { get; init; }
}