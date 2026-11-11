// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Web.Hutao.RoleCombat;

namespace Snap.Hutao.Rev.Service.Hutao;

internal interface IHutaoRoleCombatService
{
    ValueTask<RoleCombatStatisticsItem> GetRoleCombatStatisticsItemAsync();
}