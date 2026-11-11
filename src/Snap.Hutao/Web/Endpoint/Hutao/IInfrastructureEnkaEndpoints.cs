// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Web.Hoyolab;

namespace Snap.Hutao.Rev.Web.Endpoint.Hutao;

internal interface IInfrastructureEnkaEndpoints : IInfrastructureRootAccess
{
    string Enka(PlayerUid uid)
    {
        return $"{((IHomaRootAccess)this).Root}/enka/{uid}";
    }

    string EnkaPlayerInfo(PlayerUid uid)
    {
        return $"{((IHomaRootAccess)this).Root}/enka/{uid}/info";
    }
}