// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Endpoint.Hutao;

// 福建
[Service(ServiceLifetime.Singleton, typeof(IHutaoEndpoints), Key = HutaoEndpointsKind.AlphaFJ)]
internal sealed class HutaoEndpointsForAlphaFJ : IHutaoEndpoints
{
    string IHomaRootAccess.Root { get => ServerHosts.Current.ApiRoot; }

    string IInfrastructureRootAccess.Root { get => $"{ServerHosts.Current.Infrastructure}/fj"; }

    string IInfrastructureRawRootAccess.RawRoot { get => ServerHosts.Current.Infrastructure; }

    public string PatchSnapHutao()
    {
        return $"{((IInfrastructureRootAccess)this).Root}/patch/alpha";
    }
}