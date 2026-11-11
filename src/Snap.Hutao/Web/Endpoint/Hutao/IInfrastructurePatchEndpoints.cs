// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Endpoint.Hutao;

internal interface IInfrastructurePatchEndpoints : IInfrastructureRootAccess
{
    string PatchYaeAchievement() => $"{((IHomaRootAccess)this).UnprefixedRoot}/patch/yae";

    string PatchSnapHutao() => $"{((IHomaRootAccess)this).UnprefixedRoot}/patch/hutao";
}