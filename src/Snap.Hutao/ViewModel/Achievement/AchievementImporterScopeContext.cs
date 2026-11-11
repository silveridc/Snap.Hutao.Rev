// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.DataTransfer;
using Snap.Hutao.Rev.Factory.ContentDialog;
using Snap.Hutao.Rev.Factory.Picker;
using Snap.Hutao.Rev.Service.Yae;

namespace Snap.Hutao.Rev.ViewModel.Achievement;

[Service(ServiceLifetime.Scoped)]
internal sealed partial class AchievementImporterScopeContext
{
    [GeneratedConstructor]
    public partial AchievementImporterScopeContext(IServiceProvider serviceProvider);

    public partial IFileSystemPickerInteraction FileSystemPickerInteraction { get; }

    public partial JsonSerializerOptions JsonSerializerOptions { get; }

    public partial IContentDialogFactory ContentDialogFactory { get; }

    public partial IClipboardProvider ClipboardProvider { get; }

    public partial IServiceProvider ServiceProvider { get; }

    public partial IYaeService YaeService { get; }

    public partial IMessenger Messenger { get; }
}