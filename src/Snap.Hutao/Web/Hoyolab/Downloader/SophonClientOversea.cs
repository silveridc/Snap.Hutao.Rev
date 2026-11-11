// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.DependencyInjection.Annotation.HttpClient;
using Snap.Hutao.Rev.Web.Endpoint.Hoyolab;
using Snap.Hutao.Rev.Web.Hoyolab.HoyoPlay.Connect.Branch;
using Snap.Hutao.Rev.Web.Request.Builder;
using Snap.Hutao.Rev.Web.Request.Builder.Abstraction;
using Snap.Hutao.Rev.Web.Response;
using System.Net.Http;

namespace Snap.Hutao.Rev.Web.Hoyolab.Downloader;

[HttpClient(HttpClientConfiguration.Default)]
internal sealed partial class SophonClientOversea : ISophonClient
{
    private readonly IHttpRequestMessageBuilderFactory httpRequestMessageBuilderFactory;
    [FromKeyed(ApiEndpointsKind.Oversea)]
    private readonly IApiEndpoints apiEndpoints;
    private readonly HttpClient httpClient;

    [GeneratedConstructor]
    public partial SophonClientOversea(IServiceProvider serviceProvider, HttpClient httpClient);

    public async ValueTask<Response<SophonBuild>> GetBuildAsync(BranchWrapper branch, CancellationToken token = default)
    {
        HttpRequestMessageBuilder builder = httpRequestMessageBuilderFactory.Create()
            .SetRequestUri(apiEndpoints.SophonChunkGetBuildByBranch(branch))
            .Get();

        Response<SophonBuild>? resp = await builder
            .SendAsync<Response<SophonBuild>>(httpClient, token)
            .ConfigureAwait(false);

        return Response.Response.DefaultIfNull(resp);
    }

    public async ValueTask<Response<SophonPatchBuild>> GetPatchBuildAsync(BranchWrapper branch, CancellationToken token = default)
    {
        HttpRequestMessageBuilder builder = httpRequestMessageBuilderFactory.Create()
            .SetRequestUri(apiEndpoints.SophonChunkGetPatchBuild())
            .PostJson(branch);

        Response<SophonPatchBuild>? resp = await builder
            .SendAsync<Response<SophonPatchBuild>>(httpClient, token)
            .ConfigureAwait(false);

        return Response.Response.DefaultIfNull(resp);
    }
}