// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Web.Hoyolab.Hk4e.Event.GachaInfo;
using System.Collections.Frozen;

namespace Snap.Hutao.Rev.Service.GachaLog;

internal static class GachaLog
{
    public static readonly FrozenSet<GachaType> QueryTypes =
    [
        GachaType.NewBie,
        GachaType.Standard,
        GachaType.ActivityAvatar,
        GachaType.ActivityWeapon,
        GachaType.ActivityCity,
    ];
}