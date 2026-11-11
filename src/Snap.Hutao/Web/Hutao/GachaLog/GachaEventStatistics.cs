// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hutao.GachaLog;

internal sealed class GachaEventStatistics
{
    /// <summary>
    /// 角色活动1
    /// </summary>
    public List<ItemCount> AvatarEvent { get; set; } = default!;

    /// <summary>
    /// 角色活动2
    /// </summary>
    public List<ItemCount> AvatarEvent2 { get; set; } = default!;

    /// <summary>
    /// 武器活动
    /// </summary>
    public List<ItemCount> WeaponEvent { get; set; } = default!;

    /// <summary>
    /// 集录祈愿
    /// </summary>
    public List<ItemCount> Chronicled { get; set; } = default!;
}