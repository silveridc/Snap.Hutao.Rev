// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Model;

internal interface IEntityAccessWithMetadata<out TEntity, out TMetadata> : IEntityAccess<TEntity>
{
    TMetadata Inner { get; }
}