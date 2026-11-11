// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Endpoint.Hutao;

[Service(ServiceLifetime.Singleton, typeof(IHutaoEndpoints), Key = HutaoEndpointsKind.Release)]
internal sealed class HutaoEndpointsForRelease : IHutaoEndpoints
{
    string IHomaRootAccess.Root { get => ServerHosts.Current.ApiRoot; }

    string IInfrastructureRootAccess.Root { get => ServerHosts.Current.Infrastructure; }

    string IInfrastructureRawRootAccess.RawRoot { get => ServerHosts.Current.Infrastructure; }
}