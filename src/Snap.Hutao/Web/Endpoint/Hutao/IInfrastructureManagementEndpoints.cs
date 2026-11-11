// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Endpoint.Hutao;

internal interface IInfrastructureManagementEndpoints : IInfrastructureRawRootAccess
{
    string AmIBanned()
    {
        return $"{((IHomaRootAccess)this).UnprefixedRoot}/mgnt/am-i-banned";
    }
}