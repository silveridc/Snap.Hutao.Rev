// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Win32;
using Snap.Hutao.Rev.Win32.Foundation;
using System.Runtime.InteropServices;

namespace Snap.Hutao.Rev.Win32;

internal delegate void HutaoNotifyIconCallback(HutaoNativeNotifyIconCallbackKind kind, RECT icon, POINT point);

// 托盘图标的纯 C# 实现，替代上游 Snap.Hutao.Native.dll 中会在关停时崩溃的窗口子类实现。
internal sealed unsafe partial class HutaoNotifyIcon
{
    private const int CallbackMessage = 0x8000 + 0x101;
    private const uint NimAdd = 0;
    private const uint NimDelete = 2;
    private const uint NifMessage = 0x1;
    private const uint NifIcon = 0x2;
    private const uint NifTip = 0x4;
    private const uint WmContextMenu = 0x7B;
    private const uint WmRButtonUp = 0x205;
    private const uint WmLButtonDown = 0x201;
    private const uint WmLButtonDoubleClick = 0x203;
    private const uint ImageIcon = 1;
    private const uint LrLoadFromFile = 0x10;
    private const int GwlUserData = -21;
    private const int SmCxSmIcon = 49;
    private const int SmCySmIcon = 50;
    private const int TipLength = 128;
    private static readonly nint HwndMessage = -3;

    private static readonly string WindowClassName = "SnapHutaoNotifyIconHost";
    private static readonly WindowProcedure Procedure = WindowProc;
    private static readonly uint TaskbarCreatedMessage = RegisterWindowMessage("TaskbarCreated");
    private static bool classRegistered;

    private readonly string iconPath;
    private GCHandle selfHandle;
    private HutaoNotifyIconCallback? callback;
    private nint window;
    private nint icon;

    public HutaoNotifyIcon(string iconPath)
    {
        this.iconPath = iconPath;
    }

    public bool IsPromoted
    {
        get
        {
            using RegistryKey? settings = Registry.CurrentUser.OpenSubKey(@"Control Panel\NotifyIconSettings");
            if (settings is null)
            {
                return true;
            }

            string? processPath = Environment.ProcessPath;
            foreach (string name in settings.GetSubKeyNames())
            {
                using RegistryKey? item = settings.OpenSubKey(name);
                if (item?.GetValue("ExecutablePath") is not string executablePath)
                {
                    continue;
                }

                if (!string.Equals(executablePath, processPath, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                return item.GetValue("IsPromoted") is int promoted && promoted is not 0;
            }

            return true;
        }
    }

    public void Create(HutaoNotifyIconCallback callback, ReadOnlySpan<char> tip)
    {
        this.callback = callback;
        EnsureWindow();
        AddIcon(tip);
    }

    public void Recreate(ReadOnlySpan<char> tip)
    {
        if (window is 0)
        {
            return;
        }

        RemoveIcon();
        AddIcon(tip);
    }

    public void Destroy()
    {
        if (window is not 0)
        {
            RemoveIcon();
            _ = DestroyWindow(window);
            window = 0;
        }

        if (icon is not 0)
        {
            _ = DestroyIcon(icon);
            icon = 0;
        }

        if (selfHandle.IsAllocated)
        {
            selfHandle.Free();
        }

        callback = null;
    }

    private void EnsureWindow()
    {
        if (window is not 0)
        {
            return;
        }

        RegisterWindowClass();

        window = CreateWindowEx(0, WindowClassName, null, 0, 0, 0, 0, 0, HwndMessage, 0, 0, 0);
        if (window is 0)
        {
            throw new InvalidOperationException($"CreateWindowEx failed: {Marshal.GetLastWin32Error()}");
        }

        selfHandle = GCHandle.Alloc(this);
        _ = SetWindowLongPtr(window, GwlUserData, GCHandle.ToIntPtr(selfHandle));
    }

    private static void RegisterWindowClass()
    {
        if (classRegistered)
        {
            return;
        }

        nint className = Marshal.StringToHGlobalUni(WindowClassName);
        WndClassEx windowClass = new()
        {
            cbSize = (uint)sizeof(WndClassEx),
            lpfnWndProc = Marshal.GetFunctionPointerForDelegate(Procedure),
            lpszClassName = className,
        };

        try
        {
            if (RegisterClassEx(ref windowClass) is 0)
            {
                throw new InvalidOperationException($"RegisterClassEx failed: {Marshal.GetLastWin32Error()}");
            }
        }
        finally
        {
            Marshal.FreeHGlobal(className);
        }

        classRegistered = true;
    }

    private void AddIcon(ReadOnlySpan<char> tip)
    {
        if (icon is 0)
        {
            icon = LoadImage(0, iconPath, ImageIcon, GetSystemMetrics(SmCxSmIcon), GetSystemMetrics(SmCySmIcon), LrLoadFromFile);
        }

        NotifyIconData data = default;
        data.cbSize = (uint)sizeof(NotifyIconData);
        data.hWnd = window;
        data.uID = 1;
        data.uFlags = NifMessage | NifIcon | NifTip;
        data.uCallbackMessage = CallbackMessage;
        data.hIcon = icon;
        SetFixedString(data.szTip, tip, TipLength);

        if (!ShellNotifyIcon(NimAdd, ref data))
        {
            throw new InvalidOperationException($"Shell_NotifyIcon(NIM_ADD) failed: {Marshal.GetLastWin32Error()}");
        }
    }

    private void RemoveIcon()
    {
        NotifyIconData data = default;
        data.cbSize = (uint)sizeof(NotifyIconData);
        data.hWnd = window;
        data.uID = 1;

        _ = ShellNotifyIcon(NimDelete, ref data);
    }

    private static void SetFixedString(char* destination, ReadOnlySpan<char> source, int capacity)
    {
        int length = Math.Min(source.Length, capacity - 1);
        source[..length].CopyTo(new Span<char>(destination, length));
    }

    private void Raise(HutaoNativeNotifyIconCallbackKind kind)
    {
        if (callback is null)
        {
            return;
        }

        callback(kind, GetIconRect(), GetCursorPosition());
    }

    private RECT GetIconRect()
    {
        NotifyIconIdentifier identifier = new()
        {
            cbSize = (uint)sizeof(NotifyIconIdentifier),
            hWnd = window,
            uID = 1,
        };

        return ShellNotifyIconGetRect(ref identifier, out RECT rect) is 0 ? rect : default;
    }

    private static POINT GetCursorPosition()
    {
        _ = GetCursorPos(out POINT point);
        return point;
    }

    private static nint WindowProc(nint hwnd, uint message, nint wParam, nint lParam)
    {
        nint userData = GetWindowLongPtr(hwnd, GwlUserData);
        if (userData is not 0 && GCHandle.FromIntPtr(userData).Target is HutaoNotifyIcon self)
        {
            if (message == CallbackMessage)
            {
                switch ((uint)lParam & 0xFFFF)
                {
                    case WmContextMenu:
                    case WmRButtonUp:
                        self.Raise(HutaoNativeNotifyIconCallbackKind.ContextMenu);
                        return 0;
                    case WmLButtonDown:
                        self.Raise(HutaoNativeNotifyIconCallbackKind.LeftButtonDown);
                        return 0;
                    case WmLButtonDoubleClick:
                        self.Raise(HutaoNativeNotifyIconCallbackKind.LeftButtonDoubleClick);
                        return 0;
                }
            }
            else if (message == TaskbarCreatedMessage)
            {
                self.Raise(HutaoNativeNotifyIconCallbackKind.TaskbarCreated);
                return 0;
            }
        }

        return DefWindowProc(hwnd, message, wParam, lParam);
    }

    private delegate nint WindowProcedure(nint hwnd, uint message, nint wParam, nint lParam);

    [SuppressMessage("", "SA1307")]
    private struct NotifyIconData
    {
        public uint cbSize;
        public nint hWnd;
        public uint uID;
        public uint uFlags;
        public uint uCallbackMessage;
        public nint hIcon;
        public fixed char szTip[TipLength];
        public uint dwState;
        public uint dwStateMask;
        public fixed char szInfo[256];
        public uint uTimeoutOrVersion;
        public fixed char szInfoTitle[64];
        public uint dwInfoFlags;
        public Guid guidItem;
        public nint hBalloonIcon;
    }

    [SuppressMessage("", "SA1307")]
    private struct NotifyIconIdentifier
    {
        public uint cbSize;
        public nint hWnd;
        public uint uID;
        public Guid guidItem;
    }

    [SuppressMessage("", "SA1307")]
    private struct WndClassEx
    {
        public uint cbSize;
        public uint style;
        public nint lpfnWndProc;
        public int cbClsExtra;
        public int cbWndExtra;
        public nint hInstance;
        public nint hIcon;
        public nint hCursor;
        public nint hbrBackground;
        public nint lpszMenuName;
        public nint lpszClassName;
        public nint hIconSm;
    }

    [LibraryImport("user32.dll", EntryPoint = "RegisterClassExW", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.U2)]
    private static partial ushort RegisterClassEx(ref WndClassEx windowClass);

    [LibraryImport("user32.dll", EntryPoint = "CreateWindowExW", StringMarshalling = StringMarshalling.Utf16, SetLastError = true)]
    private static partial nint CreateWindowEx(uint exStyle, string className, string? windowName, uint style, int x, int y, int width, int height, nint parent, nint menu, nint instance, nint param);

    [LibraryImport("user32.dll", EntryPoint = "DestroyWindow", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool DestroyWindow(nint hwnd);

    [LibraryImport("user32.dll", EntryPoint = "DefWindowProcW")]
    private static partial nint DefWindowProc(nint hwnd, uint message, nint wParam, nint lParam);

    [LibraryImport("user32.dll", EntryPoint = "GetWindowLongPtrW", SetLastError = true)]
    private static partial nint GetWindowLongPtr(nint hwnd, int index);

    [LibraryImport("user32.dll", EntryPoint = "SetWindowLongPtrW", SetLastError = true)]
    private static partial nint SetWindowLongPtr(nint hwnd, int index, nint value);

    [LibraryImport("user32.dll", EntryPoint = "GetSystemMetrics")]
    private static partial int GetSystemMetrics(int index);

    [LibraryImport("user32.dll", EntryPoint = "RegisterWindowMessageW", StringMarshalling = StringMarshalling.Utf16)]
    private static partial uint RegisterWindowMessage(string name);

    [LibraryImport("user32.dll", EntryPoint = "GetCursorPos", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetCursorPos(out POINT point);

    [LibraryImport("user32.dll", EntryPoint = "LoadImageW", StringMarshalling = StringMarshalling.Utf16, SetLastError = true)]
    private static partial nint LoadImage(nint instance, string name, uint type, int cx, int cy, uint load);

    [LibraryImport("user32.dll", EntryPoint = "DestroyIcon", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool DestroyIcon(nint icon);

    [LibraryImport("shell32.dll", EntryPoint = "Shell_NotifyIconW", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool ShellNotifyIcon(uint message, ref NotifyIconData data);

    [LibraryImport("shell32.dll", EntryPoint = "Shell_NotifyIconGetRect", SetLastError = true)]
    private static partial int ShellNotifyIconGetRect(ref NotifyIconIdentifier identifier, out RECT rect);
}
