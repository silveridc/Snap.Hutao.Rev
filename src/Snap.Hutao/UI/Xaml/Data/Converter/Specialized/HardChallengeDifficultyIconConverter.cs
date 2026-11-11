// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core;
using Snap.Hutao.Rev.Model.Intrinsic;

namespace Snap.Hutao.Rev.UI.Xaml.Data.Converter.Specialized;

internal sealed partial class HardChallengeDifficultyIconConverter : ValueConverter<HardChallengeDifficultyLevel, Uri>
{
    public static Uri Convert(string iconName)
    {
        return InstalledLocation.GetAbsolutePath($"Resource/Icon/{iconName}.png").ToUri();
    }

    public override Uri Convert(HardChallengeDifficultyLevel from)
    {
        return InstalledLocation.GetAbsolutePath($"Resource/Icon/UI_LeyLineChallenge_Medal_{from:D}.png").ToUri();
    }
}