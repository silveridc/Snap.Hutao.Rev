// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Primitive;

namespace Snap.Hutao.Rev.Model.Calculable;

internal interface ICalculableWeapon : ICalculablePromoteLevel
{
    WeaponId WeaponId { get; }
}