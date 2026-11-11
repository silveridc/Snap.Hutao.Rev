// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using JetBrains.Annotations;
using Snap.Hutao.Rev.Core.IO;

namespace Snap.Hutao.Rev.Factory.Picker;

internal interface IFileSystemPickerInteraction
{
    ValueTask<ValueResult<bool, ValueFile>> PickFileAsync([LocalizationRequired] string? title, string? defaultFileName, string? filterName, string? filterType);

    ValueTask<ValueResult<bool, ValueFile>> SaveFileAsync([LocalizationRequired] string? title, string? defaultFileName, string? filterName, string? filterType);

    ValueTask<ValueResult<bool, string?>> PickFolderAsync([LocalizationRequired] string? title);
}
