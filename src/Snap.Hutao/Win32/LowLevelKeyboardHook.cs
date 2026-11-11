// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Win32.UI.WindowsAndMessaging;
using System.Runtime.InteropServices;

namespace Snap.Hutao.Rev.Win32;

internal delegate bool LowLevelKeyboardHookCallback(uint message, in KBDLLHOOKSTRUCT data);

// 原先由 Snap.Hutao.Native.dll 提供的低级键盘钩子，改为 C# 实现。
internal sealed unsafe partial class LowLevelKeyboardHook : IDisposable
{
    private const int WhKeyboardLl = 13;

    private static readonly HookProcedure Procedure = HookProc;
    private static nint procedurePointer;
    private static LowLevelKeyboardHookCallback? callback;

    private nint hook;

    public void Attach(LowLevelKeyboardHookCallback newCallback)
    {
        callback = newCallback;
        procedurePointer = Marshal.GetFunctionPointerForDelegate(Procedure);
        hook = SetWindowsHookEx(WhKeyboardLl, procedurePointer, GetModuleHandle(null), 0);
        if (hook is 0)
        {
            callback = null;
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
    }

    public void Detach(LowLevelKeyboardHookCallback detached)
    {
        if (callback is not null && callback != detached)
        {
            return;
        }

        Unhook();
    }

    public void Dispose()
    {
        Unhook();
    }

    private void Unhook()
    {
        if (hook is not 0)
        {
            _ = UnhookWindowsHookEx(hook);
            hook = 0;
        }

        callback = null;
    }

    private static nint HookProc(int code, nint wParam, nint lParam)
    {
        if (code >= 0 && callback is { } handler)
        {
            try
            {
                if (handler((uint)wParam, *(KBDLLHOOKSTRUCT*)lParam))
                {
                    return 1;
                }
            }
            catch
            {
                // 回调异常不能跨越本机边界（会触发 CLR fail-fast），按未处理继续传递。
            }
        }

        return CallNextHookEx(0, code, wParam, lParam);
    }

    private delegate nint HookProcedure(int code, nint wParam, nint lParam);

    [LibraryImport("user32.dll", EntryPoint = "SetWindowsHookExW", SetLastError = true)]
    private static partial nint SetWindowsHookEx(int idHook, nint procedure, nint module, uint threadId);

    [LibraryImport("user32.dll", EntryPoint = "CallNextHookEx", SetLastError = true)]
    private static partial nint CallNextHookEx(nint hook, int code, nint wParam, nint lParam);

    [LibraryImport("user32.dll", EntryPoint = "UnhookWindowsHookEx", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool UnhookWindowsHookEx(nint hook);

    [LibraryImport("kernel32.dll", EntryPoint = "GetModuleHandleW", StringMarshalling = StringMarshalling.Utf16)]
    private static partial nint GetModuleHandle(string? moduleName);
}
