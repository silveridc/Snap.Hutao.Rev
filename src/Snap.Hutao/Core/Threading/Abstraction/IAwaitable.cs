// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Core.Threading.Abstraction;

internal interface IAwaitable<out TAwaiter>
    where TAwaiter : IAwaiter
{
    TAwaiter GetAwaiter();
}

internal interface IAwaitable<out TAwaiter, out TResult>
    where TAwaiter : IAwaiter<TResult>
{
    TAwaiter GetAwaiter();
}