// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Entity;
using Snap.Hutao.Rev.Service.Abstraction;
using System.Collections.Frozen;

namespace Snap.Hutao.Rev.Service.HardChallenge;

internal interface IHardChallengeRepository : IRepository<HardChallengeEntry>
{
    void AddHardChallengeEntry(HardChallengeEntry entry);

    FrozenDictionary<uint, HardChallengeEntry> GetHardChallengeMapByUid(string uid);

    void UpdateHardChallengeEntry(HardChallengeEntry entry);
}