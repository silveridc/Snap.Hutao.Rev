// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml.Controls;
using Snap.Hutao.Rev.Core;
using Snap.Hutao.Rev.Core.LifeCycle;
using Snap.Hutao.Rev.Core.Setting;
using Snap.Hutao.Rev.Factory.ContentDialog;
using Snap.Hutao.Rev.Factory.Process;
using Snap.Hutao.Rev.Service.Game;
using Snap.Hutao.Rev.Service.Notification;
using Snap.Hutao.Rev.Web.Hutao;
using Snap.Hutao.Rev.Web.Hutao.Response;
using Snap.Hutao.Rev.Web.Response;

namespace Snap.Hutao.Rev.Service.Update;

[Service(ServiceLifetime.Singleton, typeof(IUpdateService))]
internal sealed partial class UpdateService : IUpdateService
{
    private const string UpdaterFilename = "Snap.Hutao.Deployment.exe";
    private const string UpdaterLocation = $"ms-appx:///Resource/Deployment/{UpdaterFilename}";

    // Avoid injecting services directly
    private readonly IServiceProvider serviceProvider;

    [GeneratedConstructor]
    public partial UpdateService(IServiceProvider serviceProvider);

    public string? UpdateInfo { get; set; }

    public async ValueTask<CheckUpdateResult> CheckUpdateAsync(CancellationToken token = default)
    {
        using (IServiceScope scope = serviceProvider.CreateScope())
        {
            CheckUpdateResult checkUpdateResult = new();
            try
            {
                ITaskContext taskContext = scope.ServiceProvider.GetRequiredService<ITaskContext>();
                await taskContext.SwitchToBackgroundAsync();

                HutaoInfrastructureClient infrastructureClient = scope.ServiceProvider.GetRequiredService<HutaoInfrastructureClient>();
                HutaoResponse<HutaoPackageInformation> response = await infrastructureClient.GetHutaoVersionInformationAsync(token).ConfigureAwait(false);

                if (!ResponseValidator.TryValidate(response, scope.ServiceProvider, out HutaoPackageInformation? packageInformation))
                {
                    checkUpdateResult.Kind = CheckUpdateResultKind.VersionApiInvalidResponse;
                    return checkUpdateResult;
                }

                checkUpdateResult.Kind = CheckUpdateResultKind.UpdateAvailable;
                checkUpdateResult.PackageInformation = packageInformation;

                // 服务端功能熔断：拿到下发即应用，与版本比较结果无关
                RemoteFeatureSwitch.Apply(packageInformation);

                if (!LocalSetting.Get(SettingKeys.OverrideUpdateVersionComparison, false))
                {
                    // 低于服务端配置的最低强制版本：必须更新，优先于普通比较
                    if (packageInformation.MinimumVersion is { } requiredMinimumVersion && HutaoRuntime.Version < requiredMinimumVersion)
                    {
                        checkUpdateResult.Kind = CheckUpdateResultKind.UpdateRequired;
                    }
                    else if (HutaoRuntime.Version >= checkUpdateResult.PackageInformation.Version)
                    {
                        // Launched in an updated version
                        checkUpdateResult.Kind = CheckUpdateResultKind.AlreadyUpdated;
                        return checkUpdateResult;
                    }
                }

                if (checkUpdateResult.PackageInformation.Validation is not { Length: > 0 })
                {
                    checkUpdateResult.Kind = CheckUpdateResultKind.VersionApiInvalidSha256;
                }

                return checkUpdateResult;
            }
            finally
            {
                UpdateInfo = checkUpdateResult.Kind switch
                {
                    CheckUpdateResultKind.UpdateRequired => SH.FormatViewModelSettingUpdateRequired(checkUpdateResult.PackageInformation?.Version.ToString()),
                    CheckUpdateResultKind.UpdateAvailable => SH.FormatViewModelSettingUpdateAvailable(checkUpdateResult.PackageInformation?.Version.ToString()),
                    CheckUpdateResultKind.AlreadyUpdated => SH.ViewModelSettingAlreadyUpdated,
                    CheckUpdateResultKind.VersionApiInvalidResponse or CheckUpdateResultKind.VersionApiInvalidSha256 => SH.ViewModelSettingCheckUpdateFailed,
                    _ => default,
                };
            }
        }
    }

    public async ValueTask TriggerUpdateAsync(CheckUpdateResult result, CancellationToken token = default)
    {
        if (result.Kind is not (CheckUpdateResultKind.UpdateAvailable or CheckUpdateResultKind.UpdateRequired))
        {
            return;
        }

        using (IServiceScope scope = serviceProvider.CreateScope())
        {
            ICurrentXamlWindowReference currentXamlWindowReference = scope.ServiceProvider.GetRequiredService<ICurrentXamlWindowReference>();
            IContentDialogFactory contentDialogFactory = scope.ServiceProvider.GetRequiredService<IContentDialogFactory>();
            IMessenger messenger = scope.ServiceProvider.GetRequiredService<IMessenger>();

            if (currentXamlWindowReference.Window is null)
            {
                return;
            }

            try
            {
                // 强制更新：只告知不询问，无论用户如何关闭对话框都直接启动更新器，不可跳过
                if (result.Kind is CheckUpdateResultKind.UpdateRequired)
                {
                    await contentDialogFactory
                        .CreateForConfirmAsync(
                            SH.FormatViewTitleUpdatePackageRequiredTitle(result.PackageInformation?.Version),
                            SH.ViewTitleUpdatePackageRequiredContent)
                        .ConfigureAwait(false);
                }
                else
                {
                    ContentDialogResult installUpdateUserConsentResult = await contentDialogFactory
                        .CreateForConfirmCancelAsync(
                            SH.FormatViewTitleUpdatePackageAvailableTitle(result.PackageInformation?.Version),
                            SH.ViewTitileUpdatePackageAvailableContent,
                            ContentDialogButton.Primary)
                        .ConfigureAwait(false);

                    if (installUpdateUserConsentResult is not ContentDialogResult.Primary)
                    {
                        return;
                    }
                }

#if IS_ALPHA_BUILD
                if (result.PackageInformation?.Mirrors.SingleOrDefault() is { MirrorType: Web.Hutao.HutaoPackageMirrorType.Browser } mirror)
                {
                    await Windows.System.Launcher.LaunchUriAsync(mirror.Url.ToUri());
                }
#else
                await LaunchUpdaterAsync().ConfigureAwait(false);
#endif
            }
            catch (Exception ex)
            {
                // Access to the path '?' is denied.
                // 0x80070002 无法启动服务，原因可能是已被禁用或与其相关联的设备没有启动
                // The process cannot access the file '?' because it is being used by another process.
                // 0x80070005 Attempted to perform an unauthorized operation.
                messenger.Send(InfoBarMessage.Error(ex));
            }
        }
    }

    private ValueTask LaunchUpdaterAsync()
    {
        string updaterTargetPath = HutaoRuntime.GetDataUpdateCacheDirectoryFile(UpdaterFilename);
        InstalledLocation.CopyFileFromApplicationUri(UpdaterLocation, updaterTargetPath);

        // The updater will request UAC permissions itself
        ProcessFactory.StartUsingShellExecute("update", updaterTargetPath);
        return default;
    }
}