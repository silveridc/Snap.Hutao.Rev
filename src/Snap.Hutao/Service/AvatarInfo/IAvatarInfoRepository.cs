// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Service.Abstraction;
using System.Collections.Immutable;
using EntityAvatarInfo = Snap.Hutao.Rev.Model.Entity.AvatarInfo;

namespace Snap.Hutao.Rev.Service.AvatarInfo;

internal interface IAvatarInfoRepository : IRepository<EntityAvatarInfo>
{
    void RemoveAvatarInfoRangeByUid(string uid);

    ImmutableArray<EntityAvatarInfo> GetAvatarInfoImmutableArrayByUid(string uid);
}