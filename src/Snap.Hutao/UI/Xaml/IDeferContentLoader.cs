// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml;

namespace Snap.Hutao.Rev.UI.Xaml;

internal interface IDeferContentLoader
{
    DependencyObject? Load(string name);

    void Unload(DependencyObject @object);
}