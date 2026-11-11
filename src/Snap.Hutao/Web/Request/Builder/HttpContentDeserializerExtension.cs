// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Web.Request.Builder.Abstraction;
using System.Net.Http;

namespace Snap.Hutao.Rev.Web.Request.Builder;

internal static class HttpContentDeserializerExtension
{
    extension(IHttpContentDeserializer deserializer)
    {
        public async ValueTask<T?> DeserializeAsync<T>(HttpContent? httpContent, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(deserializer);
            return (T?)await deserializer.DeserializeAsync(httpContent, typeof(T), cancellationToken).ConfigureAwait(false);
        }
    }
}