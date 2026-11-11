// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Endpoint.Hutao;

[Service(ServiceLifetime.Singleton, typeof(IHutaoEndpoints), Key = HutaoEndpointsKind.AlphaOS)]
internal sealed class HutaoEndpointsForAlphaOS : IHutaoEndpoints
{
    string IHomaRootAccess.Root { get => ServerHosts.Current.ApiRoot; }

    string IInfrastructureRootAccess.Root { get => $"{ServerHosts.Current.Infrastructure}/global"; }

    string IInfrastructureRawRootAccess.RawRoot { get => ServerHosts.Current.Infrastructure; }

    public string PatchSnapHutao()
    {
        return $"{((IInfrastructureRootAccess)this).Root}/patch/alpha";
    }
}