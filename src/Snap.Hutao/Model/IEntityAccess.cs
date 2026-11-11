// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Model;

internal interface IEntityAccess<out TEntity>
{
    TEntity Entity { get; }
}