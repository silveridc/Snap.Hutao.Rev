// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Entity;
using Snap.Hutao.Rev.Service.Abstraction;

namespace Snap.Hutao.Rev.Service.Hutao;

internal interface IObjectCacheRepository : IRepository<ObjectCacheEntry>
{
    ValueTask AddObjectCacheAsync<T>(string key, TimeSpan expire, T data)
        where T : class;

    ValueTask<T?> GetObjectOrDefaultAsync<T>(string key)
        where T : class;
}