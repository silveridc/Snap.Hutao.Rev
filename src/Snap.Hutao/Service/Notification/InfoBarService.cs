// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using CommunityToolkit.Mvvm.Input;
using Snap.Hutao.Rev.Factory.ContentDialog;
using Snap.Hutao.Rev.UI.Xaml.View.Dialog;
using System.Collections.ObjectModel;

namespace Snap.Hutao.Rev.Service.Notification;

[Service(ServiceLifetime.Singleton, typeof(IInfoBarService))]
internal sealed partial class InfoBarService : IInfoBarService, IRecipient<InfoBarMessage>
{
    private readonly ITaskContext taskContext;
    private readonly IServiceProvider serviceProvider;

    [GeneratedConstructor]
    public partial InfoBarService(IServiceProvider serviceProvider);

    [field: MaybeNull]
    public ObservableCollection<InfoBarOptions> Collection { get => LazyInitializer.EnsureInitialized(ref field, () => []); }

    public void Receive(InfoBarMessage message)
    {
        PrivateShowAsync(Prepare(InfoBarOptions.Create(message))).SafeForget();
    }

    private InfoBarOptions Prepare(InfoBarOptions options)
    {
        if (options.Details is null || options.ActionButtonCommand is not null)
        {
            return options;
        }

        options.ActionButtonContent = SH.ViewInfoBarViewDetailsContent;
        options.ActionButtonCommand = new RelayCommand(() => ShowDetailsAsync(options).SafeForget());
        return options;
    }

    private async ValueTask ShowDetailsAsync(InfoBarOptions options)
    {
        if (options.Details is not { } details)
        {
            return;
        }

        IContentDialogFactory contentDialogFactory = serviceProvider.GetRequiredService<IContentDialogFactory>();
        ExceptionDetailsDialog dialog = await contentDialogFactory
            .CreateInstanceAsync<ExceptionDetailsDialog>(serviceProvider, details)
            .ConfigureAwait(false);
        await contentDialogFactory.EnqueueAndShowAsync(dialog).ShowTask.ConfigureAwait(false);
    }

    private async ValueTask PrivateShowAsync(InfoBarOptions infoBarOptions)
    {
        await taskContext.SwitchToMainThreadAsync();

        try
        {
            Collection.Insert(0, infoBarOptions);
        }
        catch
        {
            // Ignore
        }
    }
}
