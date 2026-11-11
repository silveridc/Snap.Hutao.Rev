// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Entity;
using Snap.Hutao.Rev.Model.Primitive;

namespace Snap.Hutao.Rev.Service.Hutao;

internal interface IAvatarStrategyService
{
    ValueTask<AvatarStrategy?> GetStrategyByAvatarId(AvatarId avatarId);
}