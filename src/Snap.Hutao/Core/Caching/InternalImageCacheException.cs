// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.ExceptionService;

namespace Snap.Hutao.Rev.Core.Caching;

internal class InternalImageCacheException : Exception, IInternalException
{
    private InternalImageCacheException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    private InternalImageCacheException(string message)
        : base(message)
    {
    }

    public static InternalImageCacheException Throw(string message, Exception innerException)
    {
        throw new InternalImageCacheException(message, innerException);
    }

    public static InternalImageCacheException Throw(string message)
    {
        throw new InternalImageCacheException(message);
    }
}