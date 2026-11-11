// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Database.Abstraction;
using Snap.Hutao.Rev.Model;
using Snap.Hutao.Rev.UI.Xaml.Data;
using System.Runtime.CompilerServices;

namespace Snap.Hutao.Rev.Core.Database;

internal static class AdvancedDbCollectionViewExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IAdvancedDbCollectionView<TEntity> ToAdvancedDbCollectionView<TEntity>(this IList<TEntity> source, IServiceProvider serviceProvider)
        where TEntity : class, IPropertyValuesProvider, ISelectable
    {
        return new AdvancedDbCollectionView<TEntity>(source, serviceProvider);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IAdvancedDbCollectionView<TEntityAccess> ToAdvancedDbCollectionView<TEntityAccess, TEntity>(this IList<TEntityAccess> source, IServiceProvider serviceProvider)
        where TEntityAccess : class, IEntityAccess<TEntity>, IPropertyValuesProvider
        where TEntity : class, ISelectable
    {
        return new AdvancedDbCollectionView<TEntityAccess, TEntity>(source, serviceProvider);
    }
}