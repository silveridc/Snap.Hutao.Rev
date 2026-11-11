// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Win32.UI.WindowsAndMessaging;

namespace Snap.Hutao.Rev.UI.Input.LowLevel;

internal sealed class LowLevelKeyEventArgs
{
    public LowLevelKeyEventArgs(KBDLLHOOKSTRUCT data)
    {
        Data = data;
    }

    public bool Handled { get; set; }

    public KBDLLHOOKSTRUCT Data { get; }
}