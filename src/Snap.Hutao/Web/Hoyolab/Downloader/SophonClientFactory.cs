// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.DependencyInjection.Abstraction;

namespace Snap.Hutao.Rev.Web.Hoyolab.Downloader;

[Service(ServiceLifetime.Transient, typeof(IOverseaSupportFactory<ISophonClient>))]
internal sealed partial class SophonClientFactory : OverseaSupportFactory<ISophonClient, SophonClient, SophonClientOversea>
{
    [GeneratedConstructor(CallBaseConstructor = true)]
    public partial SophonClientFactory(IServiceProvider serviceProvider);
}