// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.ViewModel.AvatarProperty;

namespace Snap.Hutao.Rev.Service.AvatarInfo.Factory.Builder;

internal sealed class WeaponViewBuilder : IWeaponViewBuilder
{
    public WeaponView View { get; } = new();
}