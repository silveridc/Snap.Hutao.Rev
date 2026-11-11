// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Logging;
using Snap.Hutao.Rev.Factory.ContentDialog;
using Snap.Hutao.Rev.Service;
using Snap.Hutao.Rev.Service.Notification;
using Snap.Hutao.Rev.UI.Xaml.View.Dialog;

namespace Snap.Hutao.Rev.ViewModel.Setting;

[Service(ServiceLifetime.Scoped)]
internal sealed partial class SettingGeetestViewModel : Abstraction.ViewModel
{
    private readonly IContentDialogFactory contentDialogFactory;
    private readonly IServiceProvider serviceProvider;
    private readonly ITaskContext taskContext;
    private readonly AppOptions appOptions;
    private readonly IMessenger messenger;

    [GeneratedConstructor]
    public partial SettingGeetestViewModel(IServiceProvider serviceProvider);

    [Command("ConfigureGeetestUrlCommand")]
    private async Task ConfigureGeetestUrlAsync()
    {
        SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateUI("Config geetest url", "SettingGeetestViewModel.Command"));

        GeetestCustomUrlDialog dialog = await contentDialogFactory.CreateInstanceAsync<GeetestCustomUrlDialog>(serviceProvider).ConfigureAwait(false);
        (bool isOk, string template) = await dialog.GetUrlAsync().ConfigureAwait(false);

        if (!isOk)
        {
            return;
        }

        await taskContext.SwitchToMainThreadAsync();
        appOptions.GeetestCustomCompositeUrl.Value = template;
        messenger.Send(InfoBarMessage.Success(SH.ViewModelSettingGeetestCustomUrlSucceed));
    }
}