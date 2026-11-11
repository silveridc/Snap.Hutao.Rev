// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.IO;

namespace Snap.Hutao.Rev.Factory.Picker;

internal static class FileSystemPickerInteractionExtension
{
    extension(IFileSystemPickerInteraction interaction)
    {
        public ValueTask<ValueResult<bool, ValueFile>> PickFileAsync(FileSystemPickerOptions options)
        {
            return interaction.PickFileAsync(options.Title, options.DefaultFileName, options.FilterName, options.FilterType);
        }

        public ValueTask<ValueResult<bool, ValueFile>> SaveFileAsync(FileSystemPickerOptions options)
        {
            return interaction.SaveFileAsync(options.Title, options.DefaultFileName, options.FilterName, options.FilterType);
        }

        public ValueTask<ValueResult<bool, ValueFile>> PickFileAsync(string? title, string? filterName, string? filterType)
        {
            return interaction.PickFileAsync(title, null, filterName, filterType);
        }

        public ValueTask<ValueResult<bool, string?>> PickFolderAsync()
        {
            return interaction.PickFolderAsync(null);
        }
    }
}
