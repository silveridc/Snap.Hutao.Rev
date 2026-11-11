// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Service.UIGF;

internal sealed class UIGFImportOptions
{
    public required Model.InterChange.GachaLog.UIGF UIGF { get; set; }

    public required HashSet<uint> GachaArchiveUids { get; set; }
}