// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Bridge;

[ExtendedEnum]
internal enum BridgeShareSaveType
{
    [LocalizationKey(nameof(SH.WebBridgeShareSaveKindCopyToClipboard))]
    CopyToClipboard,

    [LocalizationKey(nameof(SH.WebBridgeShareSaveKindSaveAsFile))]
    SaveAsFile,
}