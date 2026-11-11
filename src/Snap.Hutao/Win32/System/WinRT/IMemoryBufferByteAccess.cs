// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Win32.Foundation;
using System.Runtime.InteropServices;

namespace Snap.Hutao.Rev.Win32.System.WinRT;

[SuppressMessage("", "SYSLIB1096")]
[ComImport]
[Guid("5B0D3235-4DBA-4D44-865E-8F1D0E4FD04D")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IMemoryBufferByteAccess
{
    [PreserveSig]
    unsafe HRESULT GetBuffer(byte** value, uint* capacity);
}