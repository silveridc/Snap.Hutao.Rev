// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.DependencyInjection.Abstraction;

namespace Snap.Hutao.Rev.Web.Hoyolab.Bbs.Home;

[Service(ServiceLifetime.Transient, typeof(IOverseaSupportFactory<IHomeClient>))]
internal sealed partial class HomeClientFactory : OverseaSupportFactory<IHomeClient, HomeClient, HomeClientOversea>
{
    [GeneratedConstructor(CallBaseConstructor = true)]
    public partial HomeClientFactory(IServiceProvider serviceProvider);
}