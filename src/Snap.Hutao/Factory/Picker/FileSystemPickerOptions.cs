// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Factory.Picker;

internal sealed class FileSystemPickerOptions
{
    public string? Title { get; init; }

    public string? DefaultFileName { get; init; }

    public string? FilterName { get; init; }

    public string? FilterType { get; init; }
}