// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Win32.Foundation;
using System.Runtime.InteropServices;

namespace Snap.Hutao.Rev.UI.Windowing;

// 原先由 Snap.Hutao.Native.dll 提供的 NonRudeHWND 窗口属性设置，改为 C# 实现。
// 该属性让 shell 认为窗口不是"抢占焦点"的，SetForegroundWindow 才能不被前台锁定拦下。
internal sealed partial class XamlWindowNonRude : IDisposable
{
    private const string PropertyName = "NonRudeHWND";

    private readonly HWND hwnd;
    private bool attached;

    public XamlWindowNonRude(HWND hwnd)
    {
        this.hwnd = hwnd;
        if (SetProp(hwnd, PropertyName, 1) is 0)
        {
            throw new InvalidOperationException($"Failed to set window property: {Marshal.GetLastPInvokeError()}");
        }

        attached = true;
    }

    public void Dispose()
    {
        if (attached)
        {
            if (IsWindow(hwnd))
            {
                // 窗口已销毁时属性随之消失，此处失败无需上抛。
                _ = RemoveProp(hwnd, PropertyName);
            }

            attached = false;
        }
    }

    [LibraryImport("user32.dll", EntryPoint = "SetPropW", StringMarshalling = StringMarshalling.Utf16, SetLastError = true)]
    private static partial nint SetProp(nint hwnd, string name, nint value);

    [LibraryImport("user32.dll", EntryPoint = "RemovePropW", StringMarshalling = StringMarshalling.Utf16, SetLastError = true)]
    private static partial nint RemoveProp(nint hwnd, string name);

    [LibraryImport("user32.dll", EntryPoint = "IsWindow")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool IsWindow(nint hwnd);
}
