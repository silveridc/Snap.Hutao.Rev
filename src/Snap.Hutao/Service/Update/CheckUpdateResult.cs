// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Web.Hutao;

namespace Snap.Hutao.Rev.Service.Update;

internal sealed class CheckUpdateResult
{
    public CheckUpdateResultKind Kind { get; set; }

    public HutaoPackageInformation? PackageInformation { get; set; }
}