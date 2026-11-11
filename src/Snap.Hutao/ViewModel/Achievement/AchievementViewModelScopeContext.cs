// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.DataTransfer;
using Snap.Hutao.Rev.Factory.ContentDialog;
using Snap.Hutao.Rev.Factory.Picker;
using Snap.Hutao.Rev.Service.Achievement;
using Snap.Hutao.Rev.Service.Metadata;

namespace Snap.Hutao.Rev.ViewModel.Achievement;

[Service(ServiceLifetime.Scoped)]
internal sealed partial class AchievementViewModelScopeContext
{
    [GeneratedConstructor]
    public partial AchievementViewModelScopeContext(IServiceProvider serviceProvider);

    public partial IFileSystemPickerInteraction FileSystemPickerInteraction { get; }

    public partial ILogger<AchievementViewModelScopeContext> Logger { get; }

    public partial JsonSerializerOptions JsonSerializerOptions { get; }

    public partial IContentDialogFactory ContentDialogFactory { get; }

    public partial AchievementImporter AchievementImporter { get; }

    public partial IAchievementService AchievementService { get; }

    public partial IClipboardProvider ClipboardProvider { get; }

    public partial IServiceProvider ServiceProvider { get; }

    public partial IMetadataService MetadataService { get; }

    public partial ITaskContext TaskContext { get; }

    public partial IMessenger Messenger { get; }
}