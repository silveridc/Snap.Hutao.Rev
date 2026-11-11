// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Core.LifeCycle;

internal interface IAppActivationActionHandlersAccess
{
    ValueTask HandleLaunchGameActionAsync(string? uid = null);
}