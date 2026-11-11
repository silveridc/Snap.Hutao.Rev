// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Dispatching;

namespace Snap.Hutao.Rev.Core.Threading;

internal interface ITaskContextUnsafe
{
    DispatcherQueue DispatcherQueue { get; }
}