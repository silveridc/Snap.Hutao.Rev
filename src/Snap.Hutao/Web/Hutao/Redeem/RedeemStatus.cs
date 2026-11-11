// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hutao.Redeem;

internal enum RedeemStatus
{
    Ok,
    Invalid,
    NotExists,
    AlreadyUsed,
    Expired,
    NotEnough,
    NoSuchUser,
    DbError,
}