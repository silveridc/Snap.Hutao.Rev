// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml.Markup;
using Snap.Hutao.Rev.Core;

namespace Snap.Hutao.Rev.UI.Xaml.Markup;

[MarkupExtensionReturnType(ReturnType = typeof(bool))]
internal sealed partial class IsProcessNotElevatedExtension : MarkupExtension
{
    protected override object ProvideValue()
    {
        return !HutaoRuntime.IsProcessElevated;
    }
}