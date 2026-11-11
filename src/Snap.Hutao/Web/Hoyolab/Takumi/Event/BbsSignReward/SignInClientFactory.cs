// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.DependencyInjection.Abstraction;

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.Event.BbsSignReward;

[Service(ServiceLifetime.Transient, typeof(IOverseaSupportFactory<ISignInClient>))]
internal sealed partial class SignInClientFactory : OverseaSupportFactory<ISignInClient, SignInClient, SignInClientOversea>
{
    [GeneratedConstructor(CallBaseConstructor = true)]
    public partial SignInClientFactory(IServiceProvider serviceProvider);
}