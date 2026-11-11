// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Web.Hutao.Redeem;

internal sealed class RedeemGenerateResult
{
    public ImmutableArray<string> Codes { get; set; }
}