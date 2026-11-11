// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Net.Http;

namespace Snap.Hutao.Rev.Web.Request.Builder.Abstraction;

internal interface IHttpContentDeserializer
{
    ValueTask<object?> DeserializeAsync(HttpContent? httpContent, Type contentType, CancellationToken cancellationToken = default);
}