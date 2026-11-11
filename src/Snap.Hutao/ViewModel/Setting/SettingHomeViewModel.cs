// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core;
using Snap.Hutao.Rev.Core.Setting;
using Snap.Hutao.Rev.Model;
using Snap.Hutao.Rev.Service;
using Snap.Hutao.Rev.Web.Hoyolab;
using System.Collections.ObjectModel;

namespace Snap.Hutao.Rev.ViewModel.Setting;

[BindableCustomPropertyProvider]
[Service(ServiceLifetime.Scoped)]
internal sealed partial class SettingHomeViewModel : Abstraction.ViewModel
{
    [GeneratedConstructor]
    public partial SettingHomeViewModel(IServiceProvider serviceProvider);

    public partial AppOptions AppOptions { get; }

    public partial RuntimeOptions RuntimeOptions { get; }

    // TODO: Replace with IObservableProperty
    public NameValue<Region>? SelectedRegion
    {
        get => field ??= Selection.Initialize(AppOptions.LazyRegions, AppOptions.Region.Value);
        set
        {
            if (SetProperty(ref field, value) && value is not null)
            {
                AppOptions.Region.Value = value.Value;
            }
        }
    }

    // TODO: Replace with IObservableProperty
    public NameValue<TimeSpan>? SelectedCalendarServerTimeZoneOffset
    {
        get => field ??= Selection.Initialize(AppOptions.LazyCalendarServerTimeZoneOffsets, AppOptions.CalendarServerTimeZoneOffset.Value);
        set
        {
            if (SetProperty(ref field, value) && value is not null)
            {
                AppOptions.CalendarServerTimeZoneOffset.Value = value.Value;
            }
        }
    }

    public ObservableCollection<SettingHomeCardViewModel>? HomeCards { get; private set; }

    partial void PostConstruct(IServiceProvider serviceProvider)
    {
        List<SettingHomeCardViewModel> viewModels =
        [
            new(SH.ViewPageSettingHomeCardItemLaunchGameHeader, SettingKeys.IsHomeCardLaunchGamePresented, SettingKeys.HomeCardLaunchGameOrder),
            new(SH.ViewPageSettingHomeCardItemgachaStatisticsHeader, SettingKeys.IsHomeCardGachaStatisticsPresented, SettingKeys.HomeCardGachaStatisticsOrder),
            new(SH.ViewPageSettingHomeCardItemAchievementHeader, SettingKeys.IsHomeCardAchievementPresented, SettingKeys.HomeCardAchievementOrder),
            new(SH.ViewPageSettingHomeCardItemDailyNoteHeader, SettingKeys.IsHomeCardDailyNotePresented, SettingKeys.HomeCardDailyNoteOrder),
            new(SH.ViewPageSettingHomeCardItemCalendarHeader, SettingKeys.IsHomeCardCalendarPresented, SettingKeys.HomeCardCalendarOrder),
            new(SH.ViewPageSettingHomeCardItemSignInHeader, SettingKeys.IsHomeCardSignInPresented, SettingKeys.HomeCardSignInOrder),
        ];

        viewModels.SortBy(v => v.Order);

        HomeCards = new SettingHomeCardObservableCollection(viewModels);
    }
}