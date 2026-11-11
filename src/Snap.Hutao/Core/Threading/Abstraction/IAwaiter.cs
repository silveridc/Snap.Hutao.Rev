// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Runtime.CompilerServices;

namespace Snap.Hutao.Rev.Core.Threading.Abstraction;

internal interface IAwaiter : INotifyCompletion
{
    bool IsCompleted { get; }

    void GetResult();
}

internal interface IAwaiter<out TResult> : INotifyCompletion
{
    bool IsCompleted { get; }

    TResult GetResult();
}