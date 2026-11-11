// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Entity;
using Snap.Hutao.Rev.Service.Abstraction;
using System.Collections.Frozen;

namespace Snap.Hutao.Rev.Service.RoleCombat;

internal interface IRoleCombatRepository : IRepository<RoleCombatEntry>
{
    void AddRoleCombatEntry(RoleCombatEntry entry);

    FrozenDictionary<uint, RoleCombatEntry> GetRoleCombatEntryMapByUid(string uid);

    void UpdateRoleCombatEntry(RoleCombatEntry entry);
}