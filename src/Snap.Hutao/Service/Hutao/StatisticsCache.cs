// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Concurrent;

namespace Snap.Hutao.Rev.Service.Hutao;

internal abstract class StatisticsCache
{
    private readonly ConcurrentDictionary<Type, Task> typeToInitializationTask = [];

    protected static IEnumerable<TResult> CurrentJoinLast<TElement, TKey, TResult>(IEnumerable<TElement> current, IEnumerable<TElement>? last, Func<TElement, TKey> keySelector, Func<TElement, TElement?, TResult> resultSelector)
        where TKey : notnull
    {
        if (last is null)
        {
            foreach (TElement element in current)
            {
                yield return resultSelector(element, default);
            }
        }
        else
        {
            Dictionary<TKey, TElement> lastMap = [];
            foreach (TElement element in last)
            {
                lastMap[keySelector(element)] = element;
            }

            foreach (TElement element in current)
            {
                yield return resultSelector(element, lastMap.GetValueOrDefault(keySelector(element)));
            }
        }
    }

    protected async ValueTask InitializeForTypeAsync<T, TContext>(TContext context, Func<TContext, Task> initialization)
    {
        Type type = typeof(T);
        Task task = typeToInitializationTask.GetOrAdd(type, _ => initialization(context));
        try
        {
            await task.ConfigureAwait(false);
        }
        catch
        {
            if (typeToInitializationTask.TryGetValue(type, out Task? current) && ReferenceEquals(current, task))
            {
                typeToInitializationTask.TryRemove(type, out _);
            }

            throw;
        }
    }
}
