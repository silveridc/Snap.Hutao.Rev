// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.ViewModel.Overlay;

internal sealed class OverlayCatalog
{
    public OverlayCatalog(string id, string icon, string name)
    {
        Id = id;
        Icon = icon;
        Name = name;
    }

    public string Id { get; }

    public string Icon { get; }

    public string Name { get; }
}