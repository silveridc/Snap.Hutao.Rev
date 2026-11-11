// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Core.Threading;

internal readonly struct SemaphoreSlimToken : IDisposable
{
    private readonly SemaphoreSlim semaphoreSlim;

    public SemaphoreSlimToken(SemaphoreSlim semaphoreSlim)
    {
        this.semaphoreSlim = semaphoreSlim;
    }

    public void Dispose()
    {
        semaphoreSlim.Release();
    }
}