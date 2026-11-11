// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Core.Threading;

internal sealed class AsyncLockValue<T>
    where T : class
{
    public AsyncLock Lock { get; } = new();

    public T? Value { get; set; }

    public Task<AsyncLock.Releaser> LockAsync()
    {
        return Lock.LockAsync();
    }
}