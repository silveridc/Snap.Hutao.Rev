// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Core.LifeCycle.InterProcess.Yae;

internal sealed class YaeDataArrayReceiver : IDisposable
{
    public ImmutableArray<YaeData> Array { get; set; } = [];

    public void Dispose()
    {
        foreach (YaeData data in Array)
        {
            data.Dispose();
        }
    }
}