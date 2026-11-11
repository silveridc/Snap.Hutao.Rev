// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core;
using Snap.Hutao.Rev.Core.ExceptionService;
using Snap.Hutao.Rev.UI.Xaml.Data.Converter;
using System.Collections.Frozen;

namespace Snap.Hutao.Rev.Model.Entity.Primitive.Converter;

internal sealed partial class CultivateTypeIconConverter : ValueConverter<CultivateType, Uri>
{
    private static readonly FrozenDictionary<string, CultivateType> LocalizedNameToCultivateType = WinRTAdaptive.ToFrozenDictionary(
    [
        KeyValuePair.Create(SH.ModelEntityPrimitiveCultivateTypeAvatarAndSkill, CultivateType.AvatarAndSkill),
        KeyValuePair.Create(SH.ModelEntityPrimitiveCultivateTypeWeapon, CultivateType.Weapon),
        KeyValuePair.Create(SH.ModelEntityPrimitiveCultivateTypeFurniture, CultivateType.Furniture),
    ]);

    public static Uri CultivateTypeNameToIconUri(string cultivateTypeName)
    {
        return CultivateTypeToIconUri(LocalizedNameToCultivateType.GetValueOrDefault(cultivateTypeName));
    }

    public static Uri CultivateTypeToIconUri(CultivateType type)
    {
        string filename = type switch
        {
            CultivateType.AvatarAndSkill => "UI_BtnIcon_PlayerGirl.png",
            CultivateType.Weapon => "UI_BagTabIcon_Weapon.png",
            CultivateType.Furniture => "UI_BtnIcon_Homeworld.png",
            _ => throw HutaoException.NotSupported(),
        };

        return InstalledLocation.GetAbsolutePath($"Resource/Icon/{filename}").ToUri();
    }

    public override Uri Convert(CultivateType from)
    {
        return CultivateTypeToIconUri(from);
    }
}