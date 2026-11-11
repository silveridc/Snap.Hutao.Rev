// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Endpoint.Hutao;

[Service(ServiceLifetime.Singleton, typeof(IHutaoEndpoints), Key = HutaoEndpointsKind.AlphaCN)]
internal sealed class HutaoEndpointsForAlphaCN : IHutaoEndpoints
{
    string IHomaRootAccess.Root { get => ServerHosts.Current.ApiRoot; }

    string IInfrastructureRootAccess.Root { get => $"{ServerHosts.Current.Infrastructure}/cn"; }

    string IInfrastructureRawRootAccess.RawRoot { get => ServerHosts.Current.Infrastructure; }

    public string PatchSnapHutao() => $"{((IInfrastructureRootAccess)this).Root}/patch/alpha";
}