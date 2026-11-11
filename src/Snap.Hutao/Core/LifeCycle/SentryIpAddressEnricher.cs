// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Web.Endpoint.Hutao;
using Snap.Hutao.Rev.Web.Hutao;
using Snap.Hutao.Rev.Web.Hutao.Response;
using System.Net.Http;
using System.Net.Http.Json;

namespace Snap.Hutao.Rev.Core.LifeCycle;

[Service(ServiceLifetime.Transient)]
internal sealed partial class SentryIpAddressEnricher
{
    private readonly IHutaoEndpointsFactory hutaoEndpointsFactory;
    private readonly HttpClient httpClient;

    [GeneratedConstructor]
    public partial SentryIpAddressEnricher(IServiceProvider serviceProvider, HttpClient httpClient);

    public async ValueTask ConfigureAsync()
    {
        try
        {
            HutaoResponse<IPInformation>? response = await httpClient
                .GetFromJsonAsync<HutaoResponse<IPInformation>>(hutaoEndpointsFactory.Create().Ip())
                .ConfigureAwait(false);

            if (response?.Data is { Ip.Length: > 0 } information)
            {
                SentrySdk.ConfigureScope(static (scope, ip) => { scope.User.IpAddress = ip; }, information.Ip);
            }
        }
        catch
        {
            // Man, what can I say?
        }
    }
}