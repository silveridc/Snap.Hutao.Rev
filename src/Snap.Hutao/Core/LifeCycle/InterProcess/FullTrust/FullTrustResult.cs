// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Core.LifeCycle.InterProcess.FullTrust;

internal abstract class FullTrustResult
{
    public bool Succeeded { get; set; }

    public string? ErrorMessage { get; set; }
}