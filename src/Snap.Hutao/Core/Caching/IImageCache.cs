// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml;
using Snap.Hutao.Rev.Core.IO;

namespace Snap.Hutao.Rev.Core.Caching;

internal interface IImageCache
{
    ValueFile GetFileFromCategoryAndName(string category, string fileName);

    ValueTask<ValueFile> GetFileFromCacheAsync(Uri uri);

    ValueTask<ValueFile> GetFileFromCacheAsync(Uri uri, ElementTheme theme);

    void Remove(Uri uriForCachedItem);
}