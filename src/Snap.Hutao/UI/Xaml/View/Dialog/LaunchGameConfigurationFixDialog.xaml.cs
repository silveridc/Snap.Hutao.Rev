// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml.Controls;
using Snap.Hutao.Rev.Factory.ContentDialog;
using Snap.Hutao.Rev.Service.Game.Scheme;

namespace Snap.Hutao.Rev.UI.Xaml.View.Dialog;

[DependencyProperty<IEnumerable<LaunchScheme>>("KnownSchemes")]
[DependencyProperty<LaunchScheme>("SelectedScheme")]
internal sealed partial class LaunchGameConfigurationFixDialog : ContentDialog
{
    private readonly IContentDialogFactory contentDialogFactory;

    [GeneratedConstructor(InitializeComponent = true)]
    public partial LaunchGameConfigurationFixDialog(IServiceProvider serviceProvider);

    public async ValueTask<ValueResult<bool, LaunchScheme?>> GetLaunchSchemeAsync()
    {
        ContentDialogResult result = await contentDialogFactory.EnqueueAndShowAsync(this).ShowTask.ConfigureAwait(false);
        await contentDialogFactory.TaskContext.SwitchToMainThreadAsync();
        return new(result is ContentDialogResult.Primary, SelectedScheme);
    }
}