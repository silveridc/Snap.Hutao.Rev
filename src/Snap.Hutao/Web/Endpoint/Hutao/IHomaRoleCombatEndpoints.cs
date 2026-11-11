// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Endpoint.Hutao;

internal interface IHomaRoleCombatEndpoints : IHomaRootAccess
{
    string RoleCombatRecordUpload()
    {
        return $"{Root}/records/role-combat";
    }

    string RoleCombatStatistics(bool last = false)
    {
        return $"{Root}/records/role-combat/statistics?Last={last}";
    }
}
