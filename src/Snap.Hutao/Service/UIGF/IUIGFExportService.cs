// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Service.UIGF;

internal interface IUIGFExportService
{
    ValueTask ExportAsync(UIGFExportOptions exportOptions, CancellationToken token);
}