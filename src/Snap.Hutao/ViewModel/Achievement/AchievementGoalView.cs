// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using Snap.Hutao.Rev.Model;
using Snap.Hutao.Rev.Model.Metadata.Achievement;
using Snap.Hutao.Rev.Model.Metadata.Converter;
using Snap.Hutao.Rev.Model.Primitive;
using Snap.Hutao.Rev.UI.Xaml.Data;

namespace Snap.Hutao.Rev.ViewModel.Achievement;

internal sealed partial class AchievementGoalView : ObservableObject,
    INameIcon<Uri>,
    IPropertyValuesProvider
{
    private AchievementGoalView(AchievementGoal goal)
    {
        Id = goal.Id;
        Order = goal.Order;
        Name = goal.Name;
        Icon = AchievementIconConverter.IconNameToUri(goal.Icon);
    }

    public AchievementGoalId Id { get; }

    public uint Order { get; }

    public string Name { get; }

    public Uri Icon { get; }

    [ObservableProperty]
    public partial double FinishPercent { get; private set; }

    [ObservableProperty]
    public partial string? FinishDescription { get; private set; }

    public static AchievementGoalView Create(AchievementGoal source)
    {
        return new(source);
    }

    public void UpdateFinishDescriptionAndPercent(AchievementGoalStatistics statistics)
    {
        FinishDescription = AchievementStatistics.Format(statistics.Finished, statistics.TotalCount, out double finishPercent);
        FinishPercent = finishPercent;
    }
}