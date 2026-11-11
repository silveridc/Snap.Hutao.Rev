// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Web.Request.Builder.Abstraction;

namespace Snap.Hutao.Rev.Web.Request.Builder;

[Service(ServiceLifetime.Singleton, typeof(IHttpRequestMessageBuilderFactory))]
internal sealed partial class HttpRequestMessageBuilderFactory : IHttpRequestMessageBuilderFactory
{
    private readonly JsonHttpContentSerializer jsonHttpContentSerializer;
    private readonly IServiceProvider serviceProvider;

    [GeneratedConstructor]
    public partial HttpRequestMessageBuilderFactory(IServiceProvider serviceProvider);

    public HttpRequestMessageBuilder Create()
    {
        return new(serviceProvider, jsonHttpContentSerializer);
    }
}