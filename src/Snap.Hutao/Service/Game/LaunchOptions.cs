// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Windowing;
using Snap.Hutao.Rev.Core;
using Snap.Hutao.Rev.Core.Property;
using Snap.Hutao.Rev.Core.Setting;
using Snap.Hutao.Rev.Model;
using Snap.Hutao.Rev.Model.Intrinsic;
using Snap.Hutao.Rev.Service.Abstraction;
using Snap.Hutao.Rev.Service.Game.FileSystem;
using Snap.Hutao.Rev.Service.Game.PathAbstraction;
using Snap.Hutao.Rev.Win32;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Service.Game;

[Service(ServiceLifetime.Singleton)]
internal sealed partial class LaunchOptions : DbStoreOptions, IRestrictedGamePathAccess
{
    [GeneratedConstructor(CallBaseConstructor = true)]
    public partial LaunchOptions(IServiceProvider serviceProvider);

    [field: MaybeNull]
    public static IObservableProperty<bool> IsGameRunning { get => field ??= GameLifeCycle.IsGameRunningProperty; }

    [field: MaybeNull]
    public static IReadOnlyObservableProperty<bool> CanKillGameProcess { get => field ??= Property.Observe(IsGameRunning, value => HutaoRuntime.IsProcessElevated && value); }

    public AsyncReaderWriterLock GamePathLock { get; } = new();

    [field: MaybeNull]
    public IObservableProperty<GamePathEntry?> GamePathEntry { get => field ??= CreateProperty(SettingKeys.LaunchGamePath, string.Empty).AsNullableSelection(GamePathEntries.Value, static entry => entry?.Path ?? string.Empty, StringComparer.OrdinalIgnoreCase).Debug("GamePathEntry"); }

    [field: MaybeNull]
    public IObservableProperty<ImmutableArray<GamePathEntry>> GamePathEntries { get => field ??= CreatePropertyForStructUsingJson(SettingKeys.LaunchGamePathEntries, ImmutableArray<GamePathEntry>.Empty); }

    [field: MaybeNull]
    public IObservableProperty<bool> UsingHoyolabAccount { get => field ??= CreateProperty(SettingKeys.LaunchUsingHoyolabAccount, false); }

    [field: MaybeNull]
    public IObservableProperty<bool> AreCommandLineArgumentsEnabled { get => field ??= CreateProperty(SettingKeys.LaunchAreCommandLineArgumentsEnabled, true).AlsoSetFalseWhenFalse(UsingHoyolabAccount); }

    [field: MaybeNull]
    public IObservableProperty<bool> IsFullScreen { get => field ??= CreateProperty(SettingKeys.LaunchIsFullScreen, false); }

    [field: MaybeNull]
    public IObservableProperty<bool> IsBorderless { get => field ??= CreateProperty(SettingKeys.LaunchIsBorderless, false); }

    [field: MaybeNull]
    public IObservableProperty<bool> IsExclusive { get => field ??= CreateProperty(SettingKeys.LaunchIsExclusive, false); }

    [field: MaybeNull]
    public IObservableProperty<bool> IsScreenWidthEnabled { get => field ??= CreateProperty(SettingKeys.LaunchIsScreenWidthEnabled, true); }

    [field: MaybeNull]
    public IObservableProperty<int> ScreenWidth { get => field ??= CreateProperty(SettingKeys.LaunchScreenWidth, DisplayArea.Primary.OuterBounds.Width); }

    [field: MaybeNull]
    public IObservableProperty<bool> IsScreenHeightEnabled { get => field ??= CreateProperty(SettingKeys.LaunchIsScreenHeightEnabled, true); }

    [field: MaybeNull]
    public IObservableProperty<int> ScreenHeight { get => field ??= CreateProperty(SettingKeys.LaunchScreenHeight, DisplayArea.Primary.OuterBounds.Height); }

    [field: MaybeNull]
    public IObservableProperty<bool> IsMonitorEnabled { get => field ??= CreateProperty(SettingKeys.LaunchIsMonitorEnabled, true); }

    public ImmutableArray<NameValue<int>> Monitors { get; } = InitializeMonitors();

    [field: MaybeNull]
    public IObservableProperty<NameValue<int>?> Monitor { get => field ??= CreatePropertyForSelectedOneBasedIndex(SettingKeys.LaunchMonitor, Monitors); }

    [field: MaybeNull]
    public IObservableProperty<bool> IsPlatformTypeEnabled { get => field ??= CreateProperty(SettingKeys.LaunchIsPlatformTypeEnabled, false); }

    public ImmutableArray<NameValue<PlatformType>> PlatformTypes { get; } = ImmutableCollectionsNameValue.FromEnum<PlatformType>();

    [field: MaybeNull]
    public IObservableProperty<PlatformType> PlatformType { get => field ??= CreateProperty(SettingKeys.LaunchPlatformType, Model.Intrinsic.PlatformType.PC); }

    [field: MaybeNull]
    public IObservableProperty<bool> IsWindowsHDREnabled { get => field ??= CreateProperty(SettingKeys.LaunchIsWindowsHDREnabled, false); }

    [field: MaybeNull]
    public IObservableProperty<bool> UsingStarwardPlayTimeStatistics { get => field ??= CreateProperty(SettingKeys.LaunchUsingStarwardPlayTimeStatistics, false); }

    [field: MaybeNull]
    public IObservableProperty<bool> UsingBetterGenshinImpactAutomation { get => field ??= CreateProperty(SettingKeys.LaunchUsingBetterGenshinImpactAutomation, false); }

    [field: MaybeNull]
    public IObservableProperty<bool> IsIslandEnabled { get => field ??= new ForceDisabledPropertyWrapper(CreateProperty(SettingKeys.LaunchIsIslandEnabled, false), RemoteFeatureSwitch.IsIslandDisabled.AsReadOnly()); }

    public IObservableProperty<bool> IsIslandDisabled => RemoteFeatureSwitch.IsIslandDisabled;

    public IObservableProperty<string> IslandDisabledReason => RemoteFeatureSwitch.IslandDisabledReason;

    [field: MaybeNull]
    public static IObservableProperty<bool> IsIslandConnected { get => field ??= Property.CreateObservable(false); }

    [field: MaybeNull]
    public IObservableProperty<bool> IsSetFieldOfViewEnabled { get => field ??= CreateProperty(SettingKeys.LaunchIsSetFieldOfViewEnabled, true); }

    [field: MaybeNull]
    public IObservableProperty<float> TargetFov { get => field ??= CreateProperty(SettingKeys.LaunchTargetFov, 45f); }

    [field: MaybeNull]
    public IObservableProperty<bool> FixLowFovScene { get => field ??= CreateProperty(SettingKeys.LaunchFixLowFovScene, true); }

    [field: MaybeNull]
    public IObservableProperty<bool> DisableFog { get => field ??= CreateProperty(SettingKeys.LaunchDisableFogRendering, false); }

    [field: MaybeNull]
    public IObservableProperty<bool> IsSetTargetFrameRateEnabled { get => field ??= new ForceDisabledPropertyWrapper(CreateProperty(SettingKeys.LaunchIsSetTargetFrameRateEnabled, true), RemoteFeatureSwitch.IsFpsUnlockDisabled.AsReadOnly()); }

    public IObservableProperty<bool> IsFpsUnlockDisabled => RemoteFeatureSwitch.IsFpsUnlockDisabled;

    public IObservableProperty<string> FpsUnlockDisabledReason => RemoteFeatureSwitch.FpsUnlockDisabledReason;

    [field: MaybeNull]
    public IObservableProperty<int> TargetFps { get => field ??= CreateProperty(SettingKeys.LaunchTargetFps, InitializeTargetFpsWithScreenFps); }

    [field: MaybeNull]
    public IObservableProperty<bool> IsPowerSaveEnabled { get => field ??= CreateProperty(SettingKeys.LaunchIsPowerSaveEnabled, false); }

    [field: MaybeNull]
    public IObservableProperty<bool> RemoveOpenTeamProgress { get => field ??= CreateProperty(SettingKeys.LaunchRemoveOpenTeamProgress, false); }

    [field: MaybeNull]
    public IObservableProperty<bool> HideQuestBanner { get => field ??= CreateProperty(SettingKeys.LaunchHideQuestBanner, false); }

    [field: MaybeNull]
    public IObservableProperty<bool> DisableEventCameraMove { get => field ??= CreateProperty(SettingKeys.LaunchDisableEventCameraMove, false); }

    [field: MaybeNull]
    public IObservableProperty<bool> DisableShowDamageText { get => field ??= CreateProperty(SettingKeys.LaunchDisableShowDamageText, false); }

    [field: MaybeNull]
    public IObservableProperty<bool> UsingTouchScreen { get => field ??= CreateProperty(SettingKeys.LaunchUsingTouchScreen, false); }

    [field: MaybeNull]
    public IObservableProperty<bool> RedirectCombineEntry { get => field ??= CreateProperty(SettingKeys.LaunchRedirectCombineEntry, false); }

    [field: MaybeNull]
    public IObservableProperty<bool> ResinListItemId000106Allowed { get => field ??= CreateProperty(SettingKeys.LaunchResinListItemId000106Allowed, true); }

    [field: MaybeNull]
    public IObservableProperty<bool> ResinListItemId000201Allowed { get => field ??= CreateProperty(SettingKeys.LaunchResinListItemId000201Allowed, true); }

    [field: MaybeNull]
    public IObservableProperty<bool> ResinListItemId107009Allowed { get => field ??= CreateProperty(SettingKeys.LaunchResinListItemId107009Allowed, true); }

    [field: MaybeNull]
    public IObservableProperty<bool> ResinListItemId107012Allowed { get => field ??= CreateProperty(SettingKeys.LaunchResinListItemId107012Allowed, true); }

    [field: MaybeNull]
    public IObservableProperty<bool> ResinListItemId220007Allowed { get => field ??= CreateProperty(SettingKeys.LaunchResinListItemId220007Allowed, true); }

    [field: MaybeNull]
    public IObservableProperty<bool> DisablePlayerDiveMosaic { get => field ??= CreateProperty(SettingKeys.LaunchDisablePlayerDiveMosaic, false); }

    [field: MaybeNull]
    public IObservableProperty<bool> DisplayPaimon { get => field ??= CreateProperty(SettingKeys.LaunchDisplayPaimon, false); }

    [field: MaybeNull]
    public IObservableProperty<bool> HidePlayerInfo { get => field ??= CreateProperty(SettingKeys.LaunchHidePlayerInfo, false); }

    [field: MaybeNull]
    public IObservableProperty<bool> HideGrass { get => field ??= CreateProperty(SettingKeys.LaunchHideGrass, false); }

    [field: MaybeNull]
    public IObservableProperty<bool> GamepadHotSwitch { get => field ??= CreateProperty(SettingKeys.LaunchGamepadHotSwitch, false); }

    [field: MaybeNull]
    public IObservableProperty<bool> InLevelClockPageSpeedUp { get => field ??= CreateProperty(SettingKeys.LaunchInLevelClockPageSpeedUp, false); }

    [field: MaybeNull]
    public IObservableProperty<bool> RelaxInDungeonOpenMapRestrictions { get => field ??= CreateProperty(SettingKeys.LaunchRelaxInDungeonOpenMapRestrictions, false); }

    [field: MaybeNull]
    public IObservableProperty<bool> FastSkipTalk { get => field ??= CreateProperty(SettingKeys.LaunchFastSkipTalk, false); }

    [field: MaybeNull]
    public IObservableProperty<ImmutableArray<AspectRatio>> AspectRatios { get => field ??= CreatePropertyForStructUsingJson(SettingKeys.LaunchAspectRatios, ImmutableArray<AspectRatio>.Empty); }

    public AspectRatio? SelectedAspectRatio
    {
        get;
        set
        {
            if (SetProperty(ref field, value) && value is not null)
            {
                (ScreenWidth.Value, ScreenHeight.Value) = ((int)value.Width, (int)value.Height);
            }
        }
    }

    [field: MaybeNull]
    public IObservableProperty<bool> UsingOverlay { get => field ??= CreateProperty(SettingKeys.LaunchUsingOverlay, false); }

    private static int InitializeTargetFpsWithScreenFps()
    {
        return NativeSystem.GetPrimaryScreenVerticalRefreshRate();
    }

    private static ImmutableArray<NameValue<int>> InitializeMonitors()
    {
        ImmutableArray<NameValue<int>>.Builder monitors = ImmutableArray.CreateBuilder<NameValue<int>>();
        try
        {
            // This list can't use foreach
            // https://github.com/microsoft/CsWinRT/issues/747
            IReadOnlyList<DisplayArea> displayAreas = DisplayArea.FindAll();
            for (int i = 0; i < displayAreas.Count; i++)
            {
                DisplayArea displayArea = displayAreas[i];
                int index = i + 1;
                monitors.Add(new($"{displayArea.DisplayId.Value:X8}:{index}", index));
            }
        }
        catch
        {
            monitors.Clear();
        }

        return monitors.ToImmutable();
    }
}