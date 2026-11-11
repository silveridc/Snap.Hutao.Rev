// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml.Controls;
using Snap.Hutao.Rev.Core;
using Snap.Hutao.Rev.Core.ExceptionService;
using Snap.Hutao.Rev.Core.Logging;
using Snap.Hutao.Rev.Factory.ContentDialog;
using Snap.Hutao.Rev.Factory.Process;
using Snap.Hutao.Rev.Service.Game;
using Snap.Hutao.Rev.Service.Game.Configuration;
using Snap.Hutao.Rev.Service.Game.FileSystem;
using Snap.Hutao.Rev.Service.Game.Launching;
using Snap.Hutao.Rev.Service.Game.Launching.Context;
using Snap.Hutao.Rev.Service.Game.Launching.Invoker;
using Snap.Hutao.Rev.Service.Game.Scheme;
using Snap.Hutao.Rev.Service.Navigation;
using Snap.Hutao.Rev.Service.Notification;
using Snap.Hutao.Rev.UI.Xaml.View.Dialog;
using Snap.Hutao.Rev.UI.Xaml.View.Page;
using Snap.Hutao.Rev.ViewModel.User;
using Snap.Hutao.Rev.Win32.Foundation;

namespace Snap.Hutao.Rev.ViewModel.Game;

[Service(ServiceLifetime.Singleton)]
internal sealed partial class LaunchGameShared
{
    private readonly IContentDialogFactory contentDialogFactory;
    private readonly INavigationService navigationService;
    private readonly IServiceProvider serviceProvider;
    private readonly LaunchOptions launchOptions;
    private readonly ITaskContext taskContext;
    private readonly IGameService gameService;
    private readonly IMessenger messenger;

    private bool resuming;

    [GeneratedConstructor]
    public partial LaunchGameShared(IServiceProvider serviceProvoder);

    public LaunchScheme? GetCurrentLaunchSchemeFromConfigurationFile(bool showInfo = true)
    {
        ChannelOptions options = gameService.GetChannelOptions();

        if (options.ErrorKind is ChannelOptionsErrorKind.None)
        {
            try
            {
                return KnownLaunchSchemes.Values.Single(scheme => scheme.Equals(options));
            }
            catch (InvalidOperationException)
            {
                // 后台收集
                HutaoException.Throw($"不支持的 ChannelOptions: {options}");
            }
        }

        if (!showInfo)
        {
            return default;
        }

        InfoBarMessage? message = options.ErrorKind switch
        {
            ChannelOptionsErrorKind.ConfigurationFileNotFound => InfoBarMessage.Warning(
                SH.FormatViewModelLaunchGameConfigurationFailed(options.ErrorKind),
                SH.FormatViewModelLaunchGameConfigurationFileNotFound(options.FilePath),
                SH.ViewModelLaunchGameFixConfigurationFileButtonText,
                HandleConfigurationFileNotFoundCommand),
            ChannelOptionsErrorKind.GamePathNullOrEmpty => InfoBarMessage.Warning(
                SH.FormatViewModelLaunchGameConfigurationFailed(options.ErrorKind),
                SH.ViewModelLaunchGameGamePathNullOrEmpty,
                SH.ViewModelLaunchGameSetGamePathButtonText,
                HandleGamePathNullOrEmptyCommand),
            ChannelOptionsErrorKind.DeviceNotFound => InfoBarMessage.Warning(
                SH.FormatViewModelLaunchGameConfigurationFailed(options.ErrorKind),
                SH.ViewModelLaunchGameDeviceNotFound),
            ChannelOptionsErrorKind.GameContentCorrupted => InfoBarMessage.Warning(
                SH.FormatViewModelLaunchGameConfigurationFailed(options.ErrorKind),
                SH.FormatViewModelLaunchGameContentCorrupted(options.FilePath)),
            ChannelOptionsErrorKind.GamePathLocked => InfoBarMessage.Error(
                SH.ViewModelGameConfigurationCreateFailedGamePathLocked,
                options.FilePath ?? string.Empty),
            _ => default,
        };

        if (message is not null)
        {
            messenger.Send(message);
        }

        return default;
    }

    public async ValueTask DefaultLaunchExecutionAsync(IViewModelSupportLaunchExecution viewModel, UserAndUid? userAndUid)
    {
        // The game process can exist longer than the view model
        using (IServiceScope scope = serviceProvider.CreateScope())
        {
            DefaultLaunchExecutionInvoker invoker = new();
            try
            {
                if (!await EnsureInjectionElevationAsync().ConfigureAwait(false))
                {
                    return;
                }

                LaunchExecutionInvocationContext context = new()
                {
                    ViewModel = viewModel,
                    ServiceProvider = scope.ServiceProvider,
                    LaunchOptions = scope.ServiceProvider.GetRequiredService<LaunchOptions>(),
                    Identity = GameIdentity.Create(userAndUid, viewModel.GameAccount),
                };

                await invoker.InvokeAsync(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                scope.ServiceProvider.GetRequiredService<IMessenger>().Send(InfoBarMessage.Error(ex));
            }
        }
    }

    public async ValueTask ResumeLaunchExecutionAsync(IViewModelSupportLaunchExecution viewModel)
    {
        if (Interlocked.Exchange(ref resuming, true))
        {
            return;
        }

        try
        {
            if (!await GameLifeCycle.IsGameRunningAsync(taskContext).ConfigureAwait(false))
            {
                return;
            }

            if (AbstractLaunchExecutionInvoker.Invoking())
            {
                return;
            }

            if (GetCurrentLaunchSchemeFromConfigurationFile(false) is null)
            {
                return;
            }

            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                try
                {
                    LaunchExecutionInvocationContext context = new()
                    {
                        ViewModel = viewModel,
                        ServiceProvider = scope.ServiceProvider,
                        LaunchOptions = launchOptions,
                        Identity = GameIdentity.Create(),
                    };

                    await new ResumeLaunchExecutionInvoker().InvokeAsync(context).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    messenger.Send(InfoBarMessage.Error(ex));
                }
            }
        }
        finally
        {
            Volatile.Write(ref resuming, false);
        }
    }

    public async ValueTask ConvertLaunchExecutionAsync(IViewModelSupportLaunchExecution viewModel)
    {
        ConvertOnlyLaunchExecutionInvoker invoker = new();
        try
        {
            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                LaunchExecutionInvocationContext context = new()
                {
                    ViewModel = viewModel,
                    ServiceProvider = scope.ServiceProvider,
                    LaunchOptions = launchOptions,
                    Identity = GameIdentity.Create(),
                };

                await invoker.InvokeAsync(context).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            messenger.Send(InfoBarMessage.Error(ex));
        }
    }

    private async ValueTask<bool> EnsureInjectionElevationAsync()
    {
        if (HutaoRuntime.IsProcessElevated || (!launchOptions.IsIslandEnabled.Value && !launchOptions.IsSetTargetFrameRateEnabled.Value))
        {
            return true;
        }

        ContentDialogResult result = await contentDialogFactory
            .CreateForConfirmCancelAsync(SH.ViewControlElevationText, $"{SH.ServiceGameIslandNotElevated}\n\n{SH.ViewModelLaunchGameElevationRestartHint}", ContentDialogButton.Primary, primaryButtonText: SH.ViewPageSettingElevatedModeRestartAction)
            .ConfigureAwait(false);

        if (result is not ContentDialogResult.Primary)
        {
            return false;
        }

        try
        {
            SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateUI("Restart as elevated for injection", "LaunchGameShared"));
            ProcessFactory.StartUsingShellExecuteRunAs($"shell:AppsFolder\\{HutaoRuntime.FamilyName}!App");
        }
        catch (Win32Exception ex) when (ex.NativeErrorCode is (int)WIN32_ERROR.ERROR_CANCELLED)
        {
            // 用户在 UAC 提示中取消
            return false;
        }

        // 新实例完成激活后，当前实例会由私有命名管道退出
        return false;
    }

    [Command("HandleConfigurationFileNotFoundCommand")]
    private async Task HandleConfigurationFileNotFoundAsync()
    {
        SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateUI("Generate config file", "LaunchGameShared.Command"));

        const string LockTrace = $"{nameof(LaunchGameShared)}.{nameof(HandleConfigurationFileNotFoundAsync)}";
        GameFileSystemErrorKind errorKind = launchOptions.TryGetGameFileSystem(LockTrace, out IGameFileSystem? gameFileSystem);
        switch (errorKind)
        {
            case GameFileSystemErrorKind.GamePathLocked:
                messenger.Send(InfoBarMessage.Warning(SH.ViewModelGameConfigurationCreateFailed, SH.ViewModelGameConfigurationCreateFailedGamePathLocked));
                return;
            case GameFileSystemErrorKind.GamePathNullOrEmpty:
                messenger.Send(InfoBarMessage.Warning(SH.ViewModelGameConfigurationCreateFailed, SH.ViewModelGameConfigurationCreateFailedGamePathNullOrEmpty));
                return;
            default:
                ArgumentNullException.ThrowIfNull(gameFileSystem);
                break;
        }

        using (gameFileSystem)
        {
            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                LaunchGameConfigurationFixDialog dialog = await contentDialogFactory
                    .CreateInstanceAsync<LaunchGameConfigurationFixDialog>(scope.ServiceProvider)
                    .ConfigureAwait(false);

                bool isOversea = gameFileSystem.IsExecutableOversea;

                await taskContext.SwitchToMainThreadAsync();

                dialog.KnownSchemes = KnownLaunchSchemes.Values.Where(scheme => scheme.IsOversea == isOversea);
                dialog.SelectedScheme = dialog.KnownSchemes.First(scheme => scheme.IsNotCompatOnly);

                if (await dialog.GetLaunchSchemeAsync().ConfigureAwait(false) is not (true, { } launchScheme))
                {
                    return;
                }

                _ = GameConfiguration.Patch(launchScheme, gameFileSystem.ScriptVersionFilePath, gameFileSystem.GameConfigurationFilePath)
                    ? messenger.Send(InfoBarMessage.Success(SH.ViewModelLaunchGameFixConfigurationFileSucceed))
                    : messenger.Send(InfoBarMessage.Error(SH.ViewModelLaunchGameFixConfigurationFileFailed));
            }
        }
    }

    [Command("HandleGamePathNullOrEmptyCommand")]
    private void HandleGamePathNullOrEmpty()
    {
        SentrySdk.AddBreadcrumb(BreadcrumbFactory2.CreateUI("Navigate to LaunchGamePage", "LaunchGameShared.Command"));
        navigationService.Navigate<LaunchGamePage>(NavigationExtraData.Default, true);
    }
}