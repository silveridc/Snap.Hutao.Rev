// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.DependencyInjection.Annotation.HttpClient;
using Snap.Hutao.Rev.Core.IO.Http;
using Snap.Hutao.Rev.Web.Endpoint.Hutao;
using Snap.Hutao.Rev.Web.Enka.Model;
using Snap.Hutao.Rev.Web.Hoyolab;
using Snap.Hutao.Rev.Web.Request.Builder;
using Snap.Hutao.Rev.Web.Request.Builder.Abstraction;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace Snap.Hutao.Rev.Web.Enka;

[HttpClient(HttpClientConfiguration.Default)]
internal sealed partial class EnkaClient
{
    private const string EnkaAPI = "https://enka.network/api/uid/{0}";
    private const string EnkaInfoAPI = "https://enka.network/api/uid/{0}?info";

    private readonly IHttpRequestMessageBuilderFactory httpRequestMessageBuilderFactory;
    private readonly IHutaoEndpointsFactory hutaoEndpointsFactory;
    private readonly JsonSerializerOptions options;
    private readonly HttpClient httpClient;

    [GeneratedConstructor]
    public partial EnkaClient(IServiceProvider serviceProvider, HttpClient httpClient);

    public async ValueTask<EnkaResponse?> GetForwardPlayerInfoAsync(PlayerUid playerUid, CancellationToken token = default)
    {
        string url = hutaoEndpointsFactory.Create().EnkaPlayerInfo(playerUid);
        return await TryGetEnkaResponseAsync(url, true, token).ConfigureAwait(false);
    }

    public async ValueTask<EnkaResponse?> GetPlayerInfoAsync(PlayerUid playerUid, CancellationToken token = default)
    {
        return await TryGetEnkaResponseAsync(string.Format(CultureInfo.CurrentCulture, EnkaInfoAPI, playerUid), false, token).ConfigureAwait(false);
    }

    public async ValueTask<EnkaResponse?> GetForwardDataAsync(PlayerUid playerUid, CancellationToken token = default)
    {
        string url = hutaoEndpointsFactory.Create().Enka(playerUid);
        return await TryGetEnkaResponseAsync(url, true, token).ConfigureAwait(false);
    }

    public async ValueTask<EnkaResponse?> GetDataAsync(PlayerUid playerUid, CancellationToken token = default)
    {
        return await TryGetEnkaResponseAsync(string.Format(CultureInfo.CurrentCulture, EnkaAPI, playerUid), false, token).ConfigureAwait(false);
    }

    private async ValueTask<EnkaResponse?> TryGetEnkaResponseAsync(string url, bool isForward, CancellationToken token = default)
    {
        try
        {
            HttpRequestMessageBuilder builder = httpRequestMessageBuilderFactory.Create()
                .SetOptions(RetryHttpHandler.DisableRetry, true)
                .SetRequestUri(url)
                .Get();

            using (HttpResponseMessage response = await httpClient.SendAsync(builder.HttpRequestMessage, HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(false))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<EnkaResponse>(options, token).ConfigureAwait(false);
                }

                if (isForward)
                {
                    string content = await response.Content.ReadAsStringAsync(token).ConfigureAwait(false);
                    if (content.Contains("nginx", StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }
                }

                // https://github.com/yoimiya-kokomi/miao-plugin/pull/441
                // Additionally, HTTP codes for UID requests:
                // 400 = wrong UID format
                // 404 = player does not exist(MHY server told that)
                // 429 = rate - limit
                // 424 = game maintenance / everything is broken after the update
                // 500 = general server error
                // 503 = I screwed up massively
                string message = response.StatusCode switch
                {
                    HttpStatusCode.BadRequest => SH.WebEnkaResponseStatusCode400,
                    HttpStatusCode.NotFound => SH.WebEnkaResponseStatusCode404,
                    HttpStatusCode.FailedDependency => SH.WebEnkaResponseStatusCode424,
                    HttpStatusCode.TooManyRequests => SH.WebEnkaResponseStatusCode429,
                    HttpStatusCode.InternalServerError => SH.WebEnkaResponseStatusCode500,
                    HttpStatusCode.ServiceUnavailable or HttpStatusCode.GatewayTimeout => SH.WebEnkaResponseStatusCode503,
                    _ => SH.WebEnkaResponseStatusCodeUnknown,
                };

                return new() { Message = message, };
            }
        }
        catch (Exception)
        {
            return null;
        }
    }
}