// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.UI.Xaml.Data.Converter;
using Snap.Hutao.Rev.Web.Endpoint.Hutao;

namespace Snap.Hutao.Rev.Model.Metadata.Converter;

internal sealed partial class GachaAvatarImgConverter : ValueConverter<string, Uri>, IIconNameToUriConverter
{
    public static Uri IconNameToUri(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return default!;
        }

        return StaticResourcesEndpoints.StaticRaw("GachaAvatarImg", $"UI_Gacha_AvatarImg_{CommonNameExtractor.ExtractUIAvatarIconName(name)}.png").ToUri();
    }

    public override Uri Convert(string from)
    {
        return IconNameToUri(from);
    }
}