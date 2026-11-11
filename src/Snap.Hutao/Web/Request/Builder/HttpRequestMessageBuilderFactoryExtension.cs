// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Web.Request.Builder.Abstraction;

namespace Snap.Hutao.Rev.Web.Request.Builder;

internal static class HttpRequestMessageBuilderFactoryExtension
{
    extension(IHttpRequestMessageBuilderFactory factory)
    {
        public HttpRequestMessageBuilder Create(string requestUri)
        {
            return factory.Create().SetRequestUri(requestUri);
        }
    }
}