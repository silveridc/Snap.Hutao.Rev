// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Endpoint.Hutao;

internal interface IHomaRedeemCodeEndpoints : IHomaRootAccess
{
    string RedeemCodeUse()
    {
        return $"{Root}/redeem-codes/redemptions";
    }
}
