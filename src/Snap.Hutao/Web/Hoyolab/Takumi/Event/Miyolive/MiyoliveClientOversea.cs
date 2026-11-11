// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.DependencyInjection.Annotation.HttpClient;
using Snap.Hutao.Rev.Core.ExceptionService;
using Snap.Hutao.Rev.Web.Response;
using System.Net.Http;

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.Event.Miyolive;

[HttpClient(HttpClientConfiguration.Default)]
internal sealed partial class MiyoliveClientOversea : IMiyoliveClient
{
    [GeneratedConstructor]
    public partial MiyoliveClientOversea(IServiceProvider serviceProvider, HttpClient httpClient);

    public ValueTask<Response<CodeListWrapper>> RefreshCodeAsync(string actId, CancellationToken token = default)
    {
        return ValueTask.FromException<Response<CodeListWrapper>>(HutaoException.NotSupported());
    }
}