// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.DependencyInjection.Annotation.HttpClient;
using Snap.Hutao.Rev.Web.Endpoint.Hoyolab;
using Snap.Hutao.Rev.Web.Request.Builder;
using Snap.Hutao.Rev.Web.Request.Builder.Abstraction;
using Snap.Hutao.Rev.Web.Response;
using System.Net.Http;

namespace Snap.Hutao.Rev.Web.Hoyolab.Hk4e.Common.Announcement;

[HttpClient(HttpClientConfiguration.Default)]
internal sealed partial class AnnouncementClient
{
    private readonly IHttpRequestMessageBuilderFactory httpRequestMessageBuilderFactory;
    private readonly IApiEndpointsFactory apiEndpointsFactory;
    private readonly HttpClient httpClient;

    [GeneratedConstructor]
    public partial AnnouncementClient(IServiceProvider serviceProvider, HttpClient httpClient);

    public async ValueTask<Response<AnnouncementWrapper>> GetAnnouncementsAsync(string languageCode, Region region, CancellationToken token = default)
    {
        HttpRequestMessageBuilder builder = httpRequestMessageBuilderFactory.Create()
            .SetRequestUri(apiEndpointsFactory.Create(region.IsOversea()).AnnList(languageCode, region))
            .Get();

        Response<AnnouncementWrapper>? resp = await builder
            .SendAsync<Response<AnnouncementWrapper>>(httpClient, token)
            .ConfigureAwait(false);

        return Response.Response.DefaultIfNull(resp);
    }

    public async ValueTask<Response<ListWrapper<AnnouncementContent>>> GetAnnouncementContentsAsync(string languageCode, Region region, CancellationToken token = default)
    {
        HttpRequestMessageBuilder builder = httpRequestMessageBuilderFactory.Create()
            .SetRequestUri(apiEndpointsFactory.Create(region.IsOversea()).AnnContent(languageCode, region))
            .Get();

        Response<ListWrapper<AnnouncementContent>>? resp = await builder
            .SendAsync<Response<ListWrapper<AnnouncementContent>>>(httpClient, token)
            .ConfigureAwait(false);

        return Response.Response.DefaultIfNull(resp);
    }
}