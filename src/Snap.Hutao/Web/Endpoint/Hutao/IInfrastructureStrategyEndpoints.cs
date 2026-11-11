// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Primitive;

namespace Snap.Hutao.Rev.Web.Endpoint.Hutao;

internal interface IInfrastructureStrategyEndpoints : IInfrastructureRootAccess
{
    string StrategyAll()
    {
        return $"{((IHomaRootAccess)this).UnprefixedRoot}/strategy/all";
    }

    public string StrategyItem(AvatarId avatarId)
    {
        return $"{((IHomaRootAccess)this).UnprefixedRoot}/strategy/item?item_id={avatarId}";
    }
}