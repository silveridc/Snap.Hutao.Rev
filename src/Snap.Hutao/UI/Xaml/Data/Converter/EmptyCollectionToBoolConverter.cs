// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using CommunityToolkit.WinUI.Converters;

namespace Snap.Hutao.Rev.UI.Xaml.Data.Converter;

internal sealed partial class EmptyCollectionToBoolConverter : EmptyCollectionToObjectConverter
{
    public EmptyCollectionToBoolConverter()
    {
        EmptyValue = false;
        NotEmptyValue = true;
    }
}