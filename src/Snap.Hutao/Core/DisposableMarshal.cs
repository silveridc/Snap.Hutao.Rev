// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Core;

internal static class DisposableMarshal
{
    public static void DisposeAndClear<T>(ref T? disposable)
        where T : class, IDisposable
    {
        Interlocked.Exchange(ref disposable, null)?.Dispose();
    }

    public static void DisposeAndExchange<T>(ref T? disposable, T? newDisposable)
        where T : class, IDisposable
    {
        Interlocked.Exchange(ref disposable, newDisposable)?.Dispose();
    }
}