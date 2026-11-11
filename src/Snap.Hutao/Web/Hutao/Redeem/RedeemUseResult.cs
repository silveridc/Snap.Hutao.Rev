// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hutao.Redeem;

internal sealed class RedeemUseResult
{
    public RedeemStatus Status { get; set; }

    public RedeemCodeTargetServiceType Type { get; set; }

    public int Value { get; set; }

    public DateTimeOffset ExpireTime { get; set; }
}