// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.UI.Xaml.Data;

namespace Snap.Hutao.Rev.Core.Database;

internal interface IAdvancedDbCollectionView<TEntity> : IAdvancedCollectionView<TEntity>
    where TEntity : class, IPropertyValuesProvider
{
    IDisposable SuppressChangeCurrentItem();
}