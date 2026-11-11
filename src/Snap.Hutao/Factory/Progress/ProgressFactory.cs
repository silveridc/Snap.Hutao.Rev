// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.ExceptionService;

namespace Snap.Hutao.Rev.Factory.Progress;

[Service(ServiceLifetime.Transient, typeof(IProgressFactory))]
internal sealed partial class ProgressFactory : IProgressFactory
{
    private readonly ITaskContext taskContext;

    [GeneratedConstructor]
    public partial ProgressFactory(IServiceProvider serviceProvider);

    public IProgress<T> CreateForMainThread<T>(Action<T> handler)
    {
        if (taskContext is not ITaskContextUnsafe @unsafe)
        {
            throw HutaoException.NotSupported();
        }

        return new DispatcherQueueProgress<T>(handler, @unsafe.DispatcherQueue);
    }

    public IProgress<T> CreateForMainThread<T, TState>(Action<T, TState> handler, TState state)
    {
        if (taskContext is not ITaskContextUnsafe @unsafe)
        {
            throw HutaoException.NotSupported();
        }

        return new DispatcherQueueProgress<T, TState>(handler, state, @unsafe.DispatcherQueue);
    }
}