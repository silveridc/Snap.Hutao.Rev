// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Service.Abstraction;
using System.Collections.Immutable;
using EntityAvatarInfo = Snap.Hutao.Rev.Model.Entity.AvatarInfo;

namespace Snap.Hutao.Rev.Service.AvatarInfo;

[Service(ServiceLifetime.Singleton, typeof(IAvatarInfoRepository))]
internal sealed partial class AvatarInfoRepository : IAvatarInfoRepository
{
    [GeneratedConstructor]
    public partial AvatarInfoRepository(IServiceProvider serviceProvider);

    public partial IServiceProvider ServiceProvider { get; }

    public ImmutableArray<EntityAvatarInfo> GetAvatarInfoImmutableArrayByUid(string uid)
    {
        return this.ImmutableArray(i => i.Uid == uid);
    }

    public void RemoveAvatarInfoRangeByUid(string uid)
    {
        this.Delete(i => i.Uid == uid);
    }
}