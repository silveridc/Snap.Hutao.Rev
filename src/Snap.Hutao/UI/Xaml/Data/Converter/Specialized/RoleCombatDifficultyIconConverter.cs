// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core;
using Snap.Hutao.Rev.Model.Intrinsic;

namespace Snap.Hutao.Rev.UI.Xaml.Data.Converter.Specialized;

internal sealed partial class RoleCombatDifficultyIconConverter : ValueConverter<RoleCombatDifficultyLevel, Uri>
{
    public override Uri Convert(RoleCombatDifficultyLevel from)
    {
        return InstalledLocation.GetAbsolutePath($"Resource/Icon/UI_RoleCombat_Medal_S_{from:D}.png").ToUri();
    }
}