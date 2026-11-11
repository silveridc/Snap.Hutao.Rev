// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Snap.Hutao.Rev.Core;
using Snap.Hutao.Rev.Core.LifeCycle;
using Snap.Hutao.Rev.Core.Logging;
using Snap.Hutao.Rev.Core.Setting;
using Snap.Hutao.Rev.Factory.ContentDialog;
using Snap.Hutao.Rev.Service;
using Snap.Hutao.Rev.Service.BackgroundActivity;
using Snap.Hutao.Rev.Service.Metadata;
using Snap.Hutao.Rev.Service.Notification;
using Snap.Hutao.Rev.Service.Update;
using Snap.Hutao.Rev.UI.Xaml.Behavior.Action;
using Snap.Hutao.Rev.UI.Xaml.View.Window.WebView2;
using System.IO;

namespace Snap.Hutao.Rev.ViewModel;

[BindableCustomPropertyProvider]
[Service(ServiceLifetime.Transient)]
internal sealed partial class MainViewModel : Abstraction.ViewModel, IDisposable
{
    private readonly ICurrentXamlWindowReference currentXamlWindowReference;
    private readonly IContentDialogFactory contentDialogFactory;
    private readonly IMetadataService metadataService;
    private readonly IUpdateService updateService;
    private readonly ITaskContext taskContext;
    private readonly IMessenger messenger;
    private readonly App app;

    [GeneratedConstructor]
    public partial MainViewModel(IServiceProvider serviceProvider);

    public static string? Title { get => HutaoRuntime.GetDisplayName(); }

    public static bool IsDebug
    {
        get =>
#if DEBUG
            true;
#else
            false;
#endif
    }

    public partial AppOptions AppOptions { get; }

    public partial BackgroundActivityOptions BackgroundActivityOptions { get; }

    public override void Dispose()
    {
        using (CriticalSection.Enter())
        {
            Uninitialize();
        }

        base.Dispose();
    }

    protected override async ValueTask<bool> LoadOverrideAsync(CancellationToken token)
    {
        await ShowDisclaimerOnceAsync().ConfigureAwait(false);
        await ShowUpdateLogWindowAfterUpdateAsync().ConfigureAwait(false);
        NotifyIfDataFolderHasReparsePoint();
        await CheckUpdateAsync().ConfigureAwait(false);

        return true;
    }

    [Command("InvertAppThemeCommand")]
    private void InvertAppTheme()
    {
        SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateUI("Invert app theme", "MainViewModel.Command"));
        if (currentXamlWindowReference.Window?.Content is FrameworkElement content)
        {
            content.RequestedTheme = app.RequestedTheme is ApplicationTheme.Dark ? ElementTheme.Light : ElementTheme.Dark;
        }
    }

    private async ValueTask ShowDisclaimerOnceAsync()
    {
        if (LocalSetting.Get(SettingKeys.DisclaimerAcknowledged, false))
        {
            return;
        }

        SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateUI("Show disclaimer dialog", "MainViewModel"));
        if (await contentDialogFactory.CreateForConfirmAsync(SH.ViewDialogDisclaimerTitle, SH.ViewDialogDisclaimerContent).ConfigureAwait(false) is ContentDialogResult.Primary)
        {
            LocalSetting.Set(SettingKeys.DisclaimerAcknowledged, true);
        }
    }

    private async ValueTask ShowUpdateLogWindowAfterUpdateAsync()
    {
        if (LocalSetting.Get(SettingKeys.AlwaysIsFirstRunAfterUpdate, false) || XamlApplicationLifetime.IsFirstRunAfterUpdate)
        {
            // XamlRoot 只能在主线程读取,而 LoadOverrideAsync 跑在后台线程
            await taskContext.SwitchToMainThreadAsync();

            // Check if the window showed, only set to false if it is shown
            if (ShowWebView2WindowAction.TryShow<UpdateLogContentProvider>(currentXamlWindowReference.XamlRoot) is not null)
            {
                SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateUI("Show update log window", "MainViewModel.Command"));
                XamlApplicationLifetime.IsFirstRunAfterUpdate = false;
            }
        }
    }

    private async ValueTask CheckUpdateAsync()
    {
        SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateUI("Check for update", "MainViewModel.Command"));

        CheckUpdateResult checkUpdateResult = await updateService.CheckUpdateAsync().ConfigureAwait(false);
        await updateService.TriggerUpdateAsync(checkUpdateResult).ConfigureAwait(false);
    }

    private void NotifyIfDataFolderHasReparsePoint()
    {
        if (new DirectoryInfo(HutaoRuntime.DataDirectory).Attributes.HasFlag(FileAttributes.ReparsePoint))
        {
            SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateDebug("Data folder has reparse point", "MainViewModel.Command"));
            messenger.Send(InfoBarMessage.Warning(SH.FormatViewModelTitleDataFolderHasReparsepoint(HutaoRuntime.DataDirectory)));
        }
    }
}