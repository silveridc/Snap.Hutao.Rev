// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.IO;

namespace Snap.Hutao.Rev.Core.Caching;

internal interface IImageCacheDownloadOperation
{
    ValueTask DownloadFileAsync(Uri uri, ValueFile baseFile);
}