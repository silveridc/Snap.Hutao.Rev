// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using JetBrains.Annotations;

namespace Snap.Hutao.Rev.Factory.Progress;

internal interface IProgressFactory
{
    IProgress<T> CreateForMainThread<T>(Action<T> handler);

    IProgress<T> CreateForMainThread<T, TState>([RequireStaticDelegate] Action<T, TState> handler, TState state);
}