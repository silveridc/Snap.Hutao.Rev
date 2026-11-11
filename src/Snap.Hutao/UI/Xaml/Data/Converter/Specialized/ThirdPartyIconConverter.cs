// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml.Controls;
using Snap.Hutao.Rev.Core;

namespace Snap.Hutao.Rev.UI.Xaml.Data.Converter.Specialized;

internal sealed partial class ThirdPartyIconConverter : ValueConverter<string, BitmapIcon>
{
    public const string TwitterName = "X (Twitter)";

    public override BitmapIcon Convert(string from)
    {
        Uri uri = from switch
        {
            TwitterName => InstalledLocation.GetAbsolutePath("Resource/ThirdParty/Twitter.png").ToUri(),
            _ => InstalledLocation.GetAbsolutePath($"Resource/ThirdParty/{from}.png").ToUri(),
        };

        return new()
        {
            ShowAsMonochrome = false,
            UriSource = uri,
        };
    }
}