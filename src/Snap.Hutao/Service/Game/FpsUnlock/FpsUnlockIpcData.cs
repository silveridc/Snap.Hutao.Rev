// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Runtime.InteropServices;

namespace Snap.Hutao.Rev.Service.Game.FpsUnlock;

// Must match UnlockerStub.dll's IPCData layout (align(8), Pack = 8)
[StructLayout(LayoutKind.Explicit, Size = 16)]
internal struct FpsUnlockIpcData
{
    [FieldOffset(0)]
    public sbyte Status;

    [FieldOffset(4)]
    public int Framerate;

    [FieldOffset(8)]
    public byte PowerSave;

    [FieldOffset(9)]
    public byte UseMobileUI;
}
