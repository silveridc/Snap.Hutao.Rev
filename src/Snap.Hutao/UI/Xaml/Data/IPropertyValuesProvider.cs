// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.UI.Xaml.Data;

internal interface IPropertyValuesProvider
{
    object? GetPropertyValue(string name);
}