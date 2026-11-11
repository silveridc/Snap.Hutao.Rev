// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Win32.Foundation;
using System.Runtime.InteropServices;

namespace Snap.Hutao.Rev.UI.Windowing;

// 原先由 Snap.Hutao.Native.dll 提供的窗口样式与前台切换工具，改为 C# 实现。
internal static unsafe partial class WindowUtilities
{
    private const int GwlStyle = -16;
    private const int GwlExStyle = -20;
    private const int GwlpHwndParent = -8;
    private const uint WsOverlappedWindow = 0xCF0000;
    private const uint WsExLayered = 0x80000;
    private const uint WsExToolWindow = 0x80;
    private const int SwShow = 5;
    private const int SwRestore = 9;
    private const uint SwpNoSize = 0x1;
    private const uint SwpNoMove = 0x2;
    private const uint SwpNoZOrder = 0x4;
    private const uint SwpFrameChanged = 0x20;
    private const uint LwaColorKey = 0x1;
    private const uint LwaAlpha = 0x2;
    private const uint FlashwTray = 0x2;
    private const uint FlashwTimerNoFg = 0xC;
    private const uint ErrorInvalidParameter = 87;

    public static void SwitchToWindow(HWND hwnd)
    {
        if (!IsWindowVisible(hwnd))
        {
            _ = ShowWindow(hwnd, SwShow);
        }

        if (IsIconic(hwnd))
        {
            _ = ShowWindow(hwnd, SwRestore);
        }

        if (!SetForegroundWindow(hwnd))
        {
            FlashWindowInfo info = default;
            info.CbSize = (uint)sizeof(FlashWindowInfo);
            info.Hwnd = hwnd;
            info.DwFlags = FlashwTray | FlashwTimerNoFg;
            _ = FlashWindowEx(&info);
        }
    }

    public static void AddExtendedStyleLayered(HWND hwnd)
    {
        UpdateStyle(hwnd, GwlExStyle, WsExLayered, true, true);
    }

    public static void RemoveExtendedStyleLayered(HWND hwnd)
    {
        UpdateStyle(hwnd, GwlExStyle, WsExLayered, false, false);
    }

    public static void AddExtendedStyleToolWindow(HWND hwnd)
    {
        UpdateStyle(hwnd, GwlExStyle, WsExToolWindow, true, false);
    }

    public static void RemoveStyleOverlappedWindow(HWND hwnd)
    {
        UpdateStyle(hwnd, GwlStyle, WsOverlappedWindow, false, false);
    }

    public static void SetLayeredWindowTransparency(HWND hwnd, byte opacity)
    {
        if (!IsWindow(hwnd))
        {
            return;
        }

        if (!SetLayeredWindowAttributes(hwnd, 0, opacity, LwaColorKey | LwaAlpha))
        {
            throw new InvalidOperationException($"Failed to set layered window transparency: {Marshal.GetLastPInvokeError()}");
        }
    }

    public static float GetRasterizationScaleForWindow(HWND hwnd)
    {
        uint dpi = GetDpiForWindow(hwnd);
        if (dpi is 0)
        {
            throw new InvalidOperationException("Failed to get window dpi.");
        }

        // 上游把缩放率折算到百分比精度（如 1.25），保持一致以免布局尺寸出现细微差异。
        return MathF.Floor((dpi * 100f / 96f) + 0.5f) / 100f;
    }

    public static void SetWindowIsEnabled(HWND hwnd, bool isEnabled)
    {
        _ = EnableWindow(hwnd, isEnabled);
    }

    public static void SetWindowOwner(HWND hwnd, HWND owner)
    {
        Marshal.SetLastPInvokeError(0);
        _ = SetWindowLongPtr(hwnd, GwlpHwndParent, owner);
        int error = Marshal.GetLastPInvokeError();
        if (error is not 0)
        {
            throw new InvalidOperationException($"Failed to set window owner: {error}");
        }
    }

    private static void UpdateStyle(HWND hwnd, int index, uint mask, bool add, bool tolerateInvalidParameter)
    {
        if (!IsWindow(hwnd))
        {
            return;
        }

        Marshal.SetLastPInvokeError(0);
        nint style = GetWindowLongPtr(hwnd, index);
        ThrowIfLastErrorSet("Failed to get window style");
        nint maskValue = (nint)(long)mask;
        _ = SetWindowLongPtr(hwnd, index, add ? style | maskValue : style & ~maskValue);
        ThrowIfLastErrorSet("Failed to set window style");

        if (!SetWindowPos(hwnd, 0, 0, 0, 0, 0, SwpNoSize | SwpNoMove | SwpNoZOrder | SwpFrameChanged))
        {
            uint error = (uint)Marshal.GetLastPInvokeError();
            if (!(tolerateInvalidParameter && error is 0 or ErrorInvalidParameter))
            {
                throw new InvalidOperationException($"Failed to update window: {error}");
            }
        }
    }

    private static void ThrowIfLastErrorSet(string message)
    {
        int error = Marshal.GetLastPInvokeError();
        if (error is not 0)
        {
            throw new InvalidOperationException($"{message}: {error}");
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct FlashWindowInfo
    {
        public uint CbSize;
        public nint Hwnd;
        public uint DwFlags;
        public uint UcCount;
        public uint DwTimeout;
    }

    [LibraryImport("user32.dll", EntryPoint = "IsWindow")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool IsWindow(nint hwnd);

    [LibraryImport("user32.dll", EntryPoint = "IsWindowVisible")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool IsWindowVisible(nint hwnd);

    [LibraryImport("user32.dll", EntryPoint = "IsIconic")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool IsIconic(nint hwnd);

    [LibraryImport("user32.dll", EntryPoint = "ShowWindow")]
    private static partial int ShowWindow(nint hwnd, int cmdShow);

    [LibraryImport("user32.dll", EntryPoint = "SetForegroundWindow")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetForegroundWindow(nint hwnd);

    [LibraryImport("user32.dll", EntryPoint = "FlashWindowEx")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool FlashWindowEx(FlashWindowInfo* info);

    [LibraryImport("user32.dll", EntryPoint = "SetWindowPos", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetWindowPos(nint hwnd, nint hwndInsertAfter, int x, int y, int cx, int cy, uint flags);

    [LibraryImport("user32.dll", EntryPoint = "SetLayeredWindowAttributes", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetLayeredWindowAttributes(nint hwnd, uint colorKey, byte alpha, uint flags);

    [LibraryImport("user32.dll", EntryPoint = "EnableWindow")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool EnableWindow(nint hwnd, [MarshalAs(UnmanagedType.Bool)] bool enabled);

    [LibraryImport("user32.dll", EntryPoint = "GetWindowLongPtrW", SetLastError = true)]
    private static partial nint GetWindowLongPtr(nint hwnd, int index);

    [LibraryImport("user32.dll", EntryPoint = "SetWindowLongPtrW", SetLastError = true)]
    private static partial nint SetWindowLongPtr(nint hwnd, int index, nint value);

    [LibraryImport("user32.dll", EntryPoint = "GetDpiForWindow")]
    private static partial uint GetDpiForWindow(nint hwnd);
}
