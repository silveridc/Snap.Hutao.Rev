// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.UI.Xaml.Data.Converter;
using Snap.Hutao.Rev.Web.Endpoint.Hutao;

namespace Snap.Hutao.Rev.Model.Metadata.Converter;

internal sealed partial class SkillIconConverter : ValueConverter<string, Uri>, IIconNameToUriConverter
{
    public static Uri IconNameToUri(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return StaticResourcesEndpoints.UIIconNone;
        }

        return name.StartsWith("UI_Talent_", StringComparison.Ordinal)
            ? StaticResourcesEndpoints.StaticRaw("Talent", $"{name}.png").ToUri()
            : StaticResourcesEndpoints.StaticRaw("Skill", $"{name}.png").ToUri();
    }

    public override Uri Convert(string from)
    {
        return IconNameToUri(from);
    }
}