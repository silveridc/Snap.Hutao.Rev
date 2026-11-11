// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Service.Game;

internal sealed class LaunchStatus
{
    public LaunchStatus(string description)
    {
        Description = description;
    }

    public string Description { get; }
}