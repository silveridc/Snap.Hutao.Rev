// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Service.UIGF;

internal interface IUIGFImportService
{
    ValueTask ImportAsync(UIGFImportOptions importOptions, CancellationToken token);
}