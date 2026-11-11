// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.DependencyInjection.Annotation.HttpClient;
using Snap.Hutao.Rev.Web.Endpoint.Hoyolab;
using Snap.Hutao.Rev.Web.Request.Builder;
using Snap.Hutao.Rev.Web.Request.Builder.Abstraction;
using Snap.Hutao.Rev.Web.Response;
using System.Net.Http;

namespace Snap.Hutao.Rev.Web.Hoyolab.Bbs.User;

[HttpClient(HttpClientConfiguration.Default)]
[PrimaryHttpMessageHandler(UseCookies = false)]
internal sealed partial class UserClientOversea : IUserClient
{
    private readonly IHttpRequestMessageBuilderFactory httpRequestMessageBuilderFactory;
    [FromKeyed(ApiEndpointsKind.Oversea)]
    private readonly IApiEndpoints apiEndpoints;
    private readonly HttpClient httpClient;

    [GeneratedConstructor]
    public partial UserClientOversea(IServiceProvider serviceProvider, HttpClient httpClient);

    public async ValueTask<Response<UserFullInfoWrapper>> GetUserFullInfoAsync(Model.Entity.User user, CancellationToken token = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(user.Aid);

        HttpRequestMessageBuilder builder = httpRequestMessageBuilderFactory.Create()
            .SetRequestUri(apiEndpoints.UserFullInfoQuery(user.Aid))
            .SetUserCookieAndFpHeader(user, CookieType.LToken)
            .Get();

        Response<UserFullInfoWrapper>? resp = await builder
            .SendAsync<Response<UserFullInfoWrapper>>(httpClient, token)
            .ConfigureAwait(false);

        return Response.Response.DefaultIfNull(resp);
    }
}