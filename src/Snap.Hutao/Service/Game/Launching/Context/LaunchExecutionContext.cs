// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Diagnostics;

namespace Snap.Hutao.Rev.Service.Game.Launching.Context;

internal sealed class LaunchExecutionContext
{
    public required IProgress<LaunchStatus?> Progress { get; init; }

    public required IServiceProvider ServiceProvider { get; init; }

    public required ITaskContext TaskContext { get; init; }

    public required IMessenger Messenger { get; init; }

    public required LaunchOptions LaunchOptions { get; init; }

    public required IProcess Process { get; init; }

    public required bool IsOversea { get; init; }
}