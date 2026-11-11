// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Model;

internal interface INameIconSide<out TIcon> : INameIcon<TIcon>
{
    TIcon SideIcon { get; }
}