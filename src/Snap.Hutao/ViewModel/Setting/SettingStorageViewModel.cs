// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.AppLifecycle;
using Snap.Hutao.Rev.Core;
using Snap.Hutao.Rev.Core.Caching;
using Snap.Hutao.Rev.Core.Logging;
using Snap.Hutao.Rev.Core.Setting;
using Snap.Hutao.Rev.Factory.ContentDialog;
using Snap.Hutao.Rev.Factory.Picker;
using Snap.Hutao.Rev.Service.Notification;
using Snap.Hutao.Rev.ViewModel.Guide;
using Snap.Hutao.Rev.Win32.Foundation;
using System.IO;
using System.Runtime.InteropServices;
using Windows.System;

namespace Snap.Hutao.Rev.ViewModel.Setting;

[BindableCustomPropertyProvider]
[Service(ServiceLifetime.Scoped)]
internal sealed partial class SettingStorageViewModel : Abstraction.ViewModel
{
    private readonly IFileSystemPickerInteraction fileSystemPickerInteraction;
    private readonly IContentDialogFactory contentDialogFactory;
    private readonly ITaskContext taskContext;
    private readonly IMessenger messenger;

    [GeneratedConstructor]
    public partial SettingStorageViewModel(IServiceProvider serviceProvider);

    [ObservableProperty]
    public partial SettingFolderViewModel? CacheFolderView { get; set; }

    [ObservableProperty]
    public partial SettingFolderViewModel? DataFolderView { get; set; }

    public bool IsStaticResourceDownloadFailureToleranceEnabled
    {
        get => LocalSetting.Get(SettingKeys.StaticResourceDownloadFailureTolerance, false);
        set => LocalSetting.Set(SettingKeys.StaticResourceDownloadFailureTolerance, value);
    }

    [Command("OpenBackgroundImageFolderCommand")]
    private static async Task OpenBackgroundImageFolderAsync()
    {
        SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateUI("Open background image folder", "SettingStorageViewModel.Command"));
        await Launcher.LaunchFolderPathAsync(HutaoRuntime.GetDataBackgroundDirectory());
    }

    [Command("SetDataFolderCommand")]
    private async Task SetDataFolderAsync()
    {
        SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateUI("Set data folder path", "SettingStorageViewModel.Command"));

        SettingStorageSetDataFolderOperation operation = new()
        {
            FileSystemPickerInteraction = fileSystemPickerInteraction,
            ContentDialogFactory = contentDialogFactory,
            Messenger = messenger,
        };

        if (await operation.TryExecuteAsync().ConfigureAwait(false))
        {
            messenger.Send(InfoBarMessage.Success(SH.ViewModelSettingSetDataFolderSuccess));
        }
    }

    [Command("DeleteServerCacheFolderCommand")]
    private async Task DeleteServerCacheFolderAsync()
    {
        SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateUI("Delete server cache folder", "SettingStorageViewModel.Command"));

        ContentDialogResult result = await contentDialogFactory.CreateForConfirmCancelAsync(
            SH.ViewModelSettingDeleteServerCacheFolderTitle,
            SH.ViewModelSettingDeleteServerCacheFolderContent)
            .ConfigureAwait(false);

        if (result is not ContentDialogResult.Primary)
        {
            return;
        }

        string cacheFolder = HutaoRuntime.GetDataServerCacheDirectory();
        if (Directory.Exists(cacheFolder))
        {
            Directory.Delete(cacheFolder, true);
        }

        if (DataFolderView is not null)
        {
            await DataFolderView.UpdateFolderSizeTimeoutAsync(TimeSpan.FromSeconds(5)).ConfigureAwait(false);
        }

        messenger.Send(InfoBarMessage.Success(SH.ViewModelSettingActionComplete));
    }

    [Command("RefreshImageCacheCommand")]
    private async Task RefreshImageCacheAsync()
    {
        SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateUI("Refresh image cache", "SettingStorageViewModel.Command"));

        ContentDialogResult result = await contentDialogFactory.CreateForConfirmCancelAsync(
            SH.ViewModelSettingRefreshImageCacheTitle,
            SH.ViewModelSettingRefreshImageCacheContent)
            .ConfigureAwait(false);

        if (result is not ContentDialogResult.Primary)
        {
            return;
        }

        await taskContext.SwitchToBackgroundAsync();

        try
        {
            string imageCacheFolder = HutaoRuntime.GetLocalCacheImageCacheDirectory();
            if (Directory.Exists(imageCacheFolder))
            {
                Directory.Delete(imageCacheFolder, true);
            }

            // ImageCache 首次使用会自建目录，这里先补齐明暗主题子目录
            Directory.CreateDirectory(Path.Combine(imageCacheFolder, "Light"));
            Directory.CreateDirectory(Path.Combine(imageCacheFolder, "Dark"));
        }
        catch (Exception ex)
        {
            messenger.Send(InfoBarMessage.Error(SH.ViewModelSettingRefreshImageCacheFailed));
            SentrySdk.CaptureException(ex);
            return;
        }

        messenger.Send(InfoBarMessage.Success(SH.ViewModelSettingActionComplete));
    }

    [Command("ResetStaticResourceCommand")]
    private async Task ResetStaticResource()
    {
        SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateUI("Reset static resource", "SettingStorageViewModel.Command"));

        ContentDialog dialog = await contentDialogFactory
            .CreateForIndeterminateProgressAsync(SH.ViewModelSettingResetStaticResourceProgress)
            .ConfigureAwait(false);

        using (await contentDialogFactory.BlockAsync(dialog).ConfigureAwait(false))
        {
            await taskContext.SwitchToBackgroundAsync();
            StaticResource.FailAll();
            try
            {
                Directory.Delete(Path.Combine(HutaoRuntime.LocalCacheDirectory, nameof(ImageCache)), true);
            }
            catch (DirectoryNotFoundException ex)
            {
                // Could not find a part of the path '.*?'.
                if (HRESULT.IsWin32(ex.HResult, WIN32_ERROR.ERROR_PATH_NOT_FOUND))
                {
                    return;
                }

                SentrySdk.CaptureException(ex);
            }
            catch (IOException ex)
            {
                if (HRESULT.IsWin32(ex.HResult, WIN32_ERROR.ERROR_SHARING_VIOLATION))
                {
                    return;
                }

                SentrySdk.CaptureException(ex);
            }

            UnsafeLocalSetting.Set(SettingKeys.GuideState, GuideState.StaticResourceBegin);
        }

        // TODO: prompt user that restart will be non-elevated
        try
        {
            AppInstance.Restart(string.Empty);
        }
        catch (COMException ex)
        {
            if (HRESULT.IsWin32(ex.HResult, WIN32_ERROR.ERROR_PACKAGE_UPDATING))
            {
                messenger.Send(InfoBarMessage.Error(SH.ViewModelSettingStorageRestartFailed));
            }
        }
    }
}