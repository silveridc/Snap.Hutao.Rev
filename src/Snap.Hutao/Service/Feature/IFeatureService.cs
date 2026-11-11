// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Service.Yae.Achievement;

namespace Snap.Hutao.Rev.Service.Feature;

internal interface IFeatureService
{
    ValueTask<AchievementFieldId?> GetAchievementFieldIdAsync(string tag);
}