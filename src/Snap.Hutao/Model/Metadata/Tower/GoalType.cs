// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Model.Metadata.Tower;

[ExtendedEnum]
internal enum GoalType
{
    [LocalizationKey(nameof(SH.ModelMetadataTowerGoalTypeDefeatMonsters))]
    DefeatMonsters,

    [LocalizationKey(nameof(SH.ModelMetadataTowerGoalTypeDefendTarget))]
    DefendTarget,
}