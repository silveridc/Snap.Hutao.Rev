// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using JetBrains.Annotations;

namespace Snap.Hutao.Rev.Core.Threading;

internal interface ITaskContext : ITaskContextUnsafe
{
    void BeginInvokeOnMainThread([RequireStaticDelegate] Action action);

    void InvokeOnMainThread([InstantHandle] Action action);

    T InvokeOnMainThread<T>([InstantHandle] Func<T> func);

    ThreadPoolSwitchOperation SwitchToBackgroundAsync();

    DispatcherQueueSwitchOperation SwitchToMainThreadAsync();
}