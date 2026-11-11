// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.IO;
using Snap.Hutao.Rev.Factory.Picker;

namespace Snap.Hutao.Rev.Service.Game.Locator;

[Service(ServiceLifetime.Transient, typeof(IGameLocator), Key = GameLocationSourceKind.Manual)]
internal sealed partial class ManualGameLocator : IGameLocator
{
    private readonly IFileSystemPickerInteraction fileSystemPickerInteraction;

    [GeneratedConstructor]
    public partial ManualGameLocator(IServiceProvider serviceProvider);

    public async ValueTask<ValueResult<bool, string>> LocateSingleGamePathAsync()
    {
        (bool isPickerOk, ValueFile file) = await fileSystemPickerInteraction.PickFileAsync(
            SH.ServiceGameLocatorFileOpenPickerCommitText,
            SH.ServiceGameLocatorPickerFilterText,
            $"{GameConstants.YuanShenFileName};{GameConstants.GenshinImpactFileName}");

        if (isPickerOk)
        {
            string fileName = System.IO.Path.GetFileName(file);
            if (fileName.ToUpperInvariant() is GameConstants.YuanShenFileNameUpper or GameConstants.GenshinImpactFileNameUpper)
            {
                return new(true, file);
            }
        }

        return new(false, default!);
    }
}