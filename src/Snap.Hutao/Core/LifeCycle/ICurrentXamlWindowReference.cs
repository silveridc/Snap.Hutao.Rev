// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml;

namespace Snap.Hutao.Rev.Core.LifeCycle;

internal interface ICurrentXamlWindowReference
{
    Window? Window { get; set; }
}