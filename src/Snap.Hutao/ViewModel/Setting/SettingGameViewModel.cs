// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Logging;
using Snap.Hutao.Rev.Core.Setting;
using Snap.Hutao.Rev.Service;
using Snap.Hutao.Rev.Service.GachaLog.QueryProvider;
using Snap.Hutao.Rev.Service.Game;
using Snap.Hutao.Rev.Service.Notification;
using System.IO;

namespace Snap.Hutao.Rev.ViewModel.Setting;

[Service(ServiceLifetime.Scoped)]
internal sealed partial class SettingGameViewModel : Abstraction.ViewModel
{
    private readonly LaunchOptions launchOptions;
    private readonly IMessenger messenger;

    [GeneratedConstructor]
    public partial SettingGameViewModel(IServiceProvider serviceProvider);

    public partial AppOptions AppOptions { get; }

    public bool ForceUsingTouchScreenWhenIntegratedTouchPresent
    {
        get => LocalSetting.Get(SettingKeys.LaunchForceUsingTouchScreen, false);
        set => LocalSetting.Set(SettingKeys.LaunchForceUsingTouchScreen, value);
    }

    [Command("DeleteGameWebCacheCommand")]
    private void DeleteGameWebCache()
    {
        SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateUI("Delete game web cache", "SettingGameViewModel.Command"));

        string? gamePath = launchOptions.GamePathEntry.Value?.Path;

        if (string.IsNullOrEmpty(gamePath))
        {
            // TODO: show message
            return;
        }

        string cacheFilePath = GachaLogQueryWebCacheProvider.GetCacheFile(gamePath);
        string? cacheFolder = Path.GetDirectoryName(cacheFilePath);

        if (!Directory.Exists(cacheFolder))
        {
            messenger.Send(InfoBarMessage.Warning(SH.FormatViewModelSettingClearWebCachePathInvalid(cacheFolder)));
            return;
        }

        try
        {
            Directory.Delete(cacheFolder, true);
            messenger.Send(InfoBarMessage.Success(SH.ViewModelSettingClearWebCacheSuccess));
        }
        catch (UnauthorizedAccessException)
        {
            messenger.Send(InfoBarMessage.Warning(SH.ViewModelSettingClearWebCacheFail));
        }
    }
}