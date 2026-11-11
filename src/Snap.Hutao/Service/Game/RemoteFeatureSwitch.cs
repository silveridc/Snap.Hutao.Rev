// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Property;
using Snap.Hutao.Rev.Web.Hutao;

namespace Snap.Hutao.Rev.Service.Game;

internal static class RemoteFeatureSwitch
{
    [field: MaybeNull]
    public static IObservableProperty<bool> IsFpsUnlockDisabled { get => field ??= Property.CreateObservable(false); }

    [field: MaybeNull]
    public static IObservableProperty<bool> IsIslandDisabled { get => field ??= Property.CreateObservable(false); }

    [field: MaybeNull]
    public static IObservableProperty<string> FpsUnlockDisabledReason { get => field ??= Property.CreateObservable(string.Empty); }

    [field: MaybeNull]
    public static IObservableProperty<string> IslandDisabledReason { get => field ??= Property.CreateObservable(string.Empty); }

    public static void Apply(HutaoPackageInformation? packageInformation)
    {
        bool fpsUnlockDisabled = packageInformation?.FpsUnlockDisabled is true;
        bool islandDisabled = packageInformation?.IslandInjectionDisabled is true;

        IsFpsUnlockDisabled.Value = fpsUnlockDisabled;
        IsIslandDisabled.Value = islandDisabled;

        // 服务端没填原因时给一句通用说明，避免客户端展示空提示
        FpsUnlockDisabledReason.Value = fpsUnlockDisabled
            ? packageInformation!.FpsUnlockDisabledReason ?? SH.ViewPageLaunchGameFeatureDisabledDefaultReason
            : string.Empty;
        IslandDisabledReason.Value = islandDisabled
            ? packageInformation!.IslandInjectionDisabledReason ?? SH.ViewPageLaunchGameFeatureDisabledDefaultReason
            : string.Empty;
    }
}
