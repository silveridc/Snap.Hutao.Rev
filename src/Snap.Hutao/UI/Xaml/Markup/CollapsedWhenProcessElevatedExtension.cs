// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
using Snap.Hutao.Rev.Core;

namespace Snap.Hutao.Rev.UI.Xaml.Markup;

[MarkupExtensionReturnType(ReturnType = typeof(Visibility))]
internal sealed partial class CollapsedWhenProcessElevatedExtension : MarkupExtension
{
    protected override object ProvideValue()
    {
        return HutaoRuntime.IsProcessElevated ? Visibility.Collapsed : Visibility.Visible;
    }
}