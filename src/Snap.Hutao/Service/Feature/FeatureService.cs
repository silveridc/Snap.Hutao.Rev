// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Extensions.Caching.Memory;
using Snap.Hutao.Rev.Core.DependencyInjection.Annotation.HttpClient;
using Snap.Hutao.Rev.Service.Yae.Achievement;
using Snap.Hutao.Rev.Web.Endpoint.Hutao;
using Snap.Hutao.Rev.Web.Request.Builder;
using Snap.Hutao.Rev.Web.Request.Builder.Abstraction;
using System.Net.Http;

namespace Snap.Hutao.Rev.Service.Feature;

[Service(ServiceLifetime.Singleton, typeof(IFeatureService))]
[HttpClient(HttpClientConfiguration.Default)]
internal sealed partial class FeatureService : IFeatureService
{
    private readonly IHttpRequestMessageBuilderFactory httpRequestMessageBuilderFactory;
    private readonly IHutaoEndpointsFactory hutaoEndpointsFactory;
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IMemoryCache memoryCache;

    [GeneratedConstructor]
    public partial FeatureService(IServiceProvider serviceProvider);

    public async ValueTask<AchievementFieldId?> GetAchievementFieldIdAsync(string tag)
    {
        return await GetTaggedFeatureAsync<AchievementFieldId>(tag, TimeSpan.FromHours(6)).ConfigureAwait(false);
    }

    private async ValueTask<TFeature?> GetTaggedFeatureAsync<TFeature>(string tag, TimeSpan expiration)
        where TFeature : class
    {
        string featureName = typeof(TFeature).Name;
        return await memoryCache.GetOrCreateAsync($"{nameof(FeatureService)}.{featureName}.{tag}", async entry =>
        {
            entry.SetSlidingExpiration(expiration);
            HttpRequestMessageBuilder builder = httpRequestMessageBuilderFactory
                .Create()
                .SetRequestUri(hutaoEndpointsFactory.Create().Feature($"{featureName}_{tag}"))
                .Get();

            using (HttpClient httpClient = httpClientFactory.CreateClient(nameof(FeatureService)))
            {
                return (await builder.SendAsync<TFeature>(httpClient, CancellationToken.None).ConfigureAwait(false)).Body;
            }
        }).ConfigureAwait(false);
    }
}