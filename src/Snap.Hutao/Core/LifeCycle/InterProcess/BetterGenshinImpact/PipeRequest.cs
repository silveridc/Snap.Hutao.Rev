// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Core.LifeCycle.InterProcess.BetterGenshinImpact;

internal sealed class PipeRequest<T>
{
    public required PipeRequestKind Kind { get; set; }

    public required T Data { get; set; }
}