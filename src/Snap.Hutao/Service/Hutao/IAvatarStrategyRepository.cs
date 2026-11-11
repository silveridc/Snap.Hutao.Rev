// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Entity;
using Snap.Hutao.Rev.Model.Primitive;
using Snap.Hutao.Rev.Service.Abstraction;

namespace Snap.Hutao.Rev.Service.Hutao;

internal interface IAvatarStrategyRepository : IRepository<AvatarStrategy>
{
    AvatarStrategy? GetStrategyByAvatarId(AvatarId avatarId);

    void AddStrategy(AvatarStrategy strategy);

    void RemoveStrategy(AvatarStrategy strategy);
}