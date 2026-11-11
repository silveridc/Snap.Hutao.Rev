// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Abstraction;

namespace Snap.Hutao.Rev.Core.Threading;

internal readonly struct ValueResult<TResult, TValue> : IDeconstruct<TResult, TValue>
{
    public readonly TResult IsOk;
    public readonly TValue Value;

    public ValueResult(TResult isOk, TValue value)
    {
        IsOk = isOk;
        Value = value;
    }

    public void Deconstruct(out TResult isOk, out TValue value)
    {
        isOk = IsOk;
        value = Value;
    }
}