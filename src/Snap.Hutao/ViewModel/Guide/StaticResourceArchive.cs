// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.ViewModel.Guide;

[ExtendedEnum]
internal enum StaticResourceArchive
{
    [LocalizationKey(nameof(SH.ViewGuideStepStaticResourceSettingMinimumOff))]
    Full,

    [LocalizationKey(nameof(SH.ViewGuideStepStaticResourceSettingMinimumOn))]
    Minimum,
}