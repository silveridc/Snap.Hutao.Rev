// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.DependencyInjection.Annotation.HttpClient;
using Snap.Hutao.Rev.Web.Endpoint.Hoyolab;
using Snap.Hutao.Rev.Web.Request.Builder;
using Snap.Hutao.Rev.Web.Request.Builder.Abstraction;
using Snap.Hutao.Rev.Web.Response;
using System.Net.Http;

namespace Snap.Hutao.Rev.Web.Hoyolab.Bbs.Home;

[HttpClient(HttpClientConfiguration.XRpc2)]
internal sealed partial class HomeClient : IHomeClient
{
    private readonly IHttpRequestMessageBuilderFactory httpRequestMessageBuilderFactory;
    [FromKeyed(ApiEndpointsKind.Chinese)]
    private readonly IApiEndpoints apiEndpoints;
    private readonly HttpClient httpClient;

    [GeneratedConstructor]
    public partial HomeClient(IServiceProvider serviceProvider, HttpClient httpClient);

    public async ValueTask<Response<NewHomeNewInfo>> GetNewHomeInfoAsync(int gid, CancellationToken token = default)
    {
        HttpRequestMessageBuilder builder = httpRequestMessageBuilderFactory.Create()
            .SetRequestUri(apiEndpoints.HomeNew(gid))
            .Get();

        Response<NewHomeNewInfo>? resp = await builder
            .SendAsync<Response<NewHomeNewInfo>>(httpClient, token)
            .ConfigureAwait(false);

        return Response.Response.DefaultIfNull(resp);
    }
}