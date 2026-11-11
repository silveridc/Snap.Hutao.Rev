// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using CommunityToolkit.WinUI.Converters;
using Microsoft.UI.Xaml;

namespace Snap.Hutao.Rev.UI.Xaml.Data.Converter;

internal sealed partial class EmptyObjectToVisibilityConverter : EmptyObjectToObjectConverter
{
    public EmptyObjectToVisibilityConverter()
    {
        EmptyValue = Visibility.Collapsed;
        NotEmptyValue = Visibility.Visible;
    }
}