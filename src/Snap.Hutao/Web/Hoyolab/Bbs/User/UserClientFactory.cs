// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.DependencyInjection.Abstraction;

namespace Snap.Hutao.Rev.Web.Hoyolab.Bbs.User;

[Service(ServiceLifetime.Transient, typeof(IOverseaSupportFactory<IUserClient>))]

internal sealed partial class UserClientFactory : OverseaSupportFactory<IUserClient, UserClient, UserClientOversea>
{
    [GeneratedConstructor(CallBaseConstructor = true)]
    public partial UserClientFactory(IServiceProvider serviceProvider);
}