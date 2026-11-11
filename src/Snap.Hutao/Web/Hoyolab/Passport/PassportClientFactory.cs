// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.DependencyInjection.Abstraction;

namespace Snap.Hutao.Rev.Web.Hoyolab.Passport;

[Service(ServiceLifetime.Transient, typeof(IOverseaSupportFactory<IPassportClient>))]
internal sealed partial class PassportClientFactory : OverseaSupportFactory<IPassportClient, PassportClient, PassportClientOversea>
{
    [GeneratedConstructor(CallBaseConstructor = true)]
    public partial PassportClientFactory(IServiceProvider serviceProvider);
}