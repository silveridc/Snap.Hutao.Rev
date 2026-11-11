// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Win32.UI.Input.KeyboardAndMouse;
using System.Runtime.InteropServices;

namespace Snap.Hutao.Rev.Win32;

// 原先由 Snap.Hutao.Native.dll 提供的「连点/连按」热键。
// 行为与原生实现一致：全局热键切换，重复输入间隔 100–150ms（随机抖动），
// 目标进程拒绝注入（GetLastError == ERROR_ACCESS_DENIED）时静默停止。
internal sealed unsafe partial class HotKeyAction : IDisposable
{
    private const uint WmHotKey = 0x312;
    private const int InputSize = 40;
    private const uint InputMouse = 0;
    private const uint InputKeyboard = 1;
    private const uint MouseLeftDown = 0x2;
    private const uint MouseLeftUp = 0x4;
    private const uint KeyEventKeyUp = 0x2;
    private const uint KeyFToggle = 0x46; // 'F'
    private const int ErrorAccessDenied = 5;
    private const int GwlUserData = -21;
    private static readonly nint HwndMessage = -3;

    private static readonly string WindowClassName = "SnapHutaoHotKeyHost";
    private static readonly WindowProcedure Procedure = WindowProc;
    private static readonly Dictionary<uint, HotKeyAction> Actions = [];
    private static readonly Lock SyncRoot = new();
    private static bool classRegistered;
    private static nint window;
    private static uint nextId;
    private static Func<bool>? beforeSwitch;

    private readonly HutaoNativeHotKeyActionKind kind;
    private readonly Action<bool> callback;

    private uint id;
    private uint virtualKey;
    private HOT_KEY_MODIFIERS modifiers;
    private bool enabled;
    private volatile bool running;
    private Thread? repeatThread;
    private bool disposed;

    public HotKeyAction(HutaoNativeHotKeyActionKind kind, Action<bool> callback)
    {
        this.kind = kind;
        this.callback = callback;
        EnsureWindow();
    }

    public static Func<bool>? BeforeSwitch
    {
        get => beforeSwitch;
        set => beforeSwitch = value;
    }

    public bool IsEnabled
    {
        get => enabled;
        set
        {
            if (enabled == value)
            {
                return;
            }

            enabled = value;
            if (enabled)
            {
                Register();
            }
            else
            {
                Unregister();
                StopRepeat();
            }
        }
    }

    public void Update(HOT_KEY_MODIFIERS newModifiers, uint newVirtualKey)
    {
        bool wasRegistered = id is not 0;
        if (wasRegistered)
        {
            Unregister();
        }

        modifiers = newModifiers;
        virtualKey = newVirtualKey;

        if (enabled)
        {
            Register();
        }
    }

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;
        Unregister();
        StopRepeat();
    }

    private void Register()
    {
        if (id is 0)
        {
            lock (SyncRoot)
            {
                id = ++nextId;
                Actions[id] = this;
            }
        }

        if (!RegisterHotKey(window, (int)id, modifiers | HOT_KEY_MODIFIERS.MOD_NOREPEAT, virtualKey))
        {
            int error = Marshal.GetLastWin32Error();
            Unregister();
            throw new Win32Exception(error);
        }
    }

    private void Unregister()
    {
        if (id is 0)
        {
            return;
        }

        _ = UnregisterHotKey(window, (int)id);
        lock (SyncRoot)
        {
            Actions.Remove(id);
        }

        id = 0;
    }

    private void Toggle()
    {
        if (running)
        {
            StopRepeat();
            callback(false);
            return;
        }

        running = true;
        repeatThread = new Thread(RunRepeat)
        {
            IsBackground = true,
            Name = "Snap.Hutao HotKeyAction",
        };

        repeatThread.Start();
        callback(true);
    }

    private void StopRepeat()
    {
        running = false;
        if (repeatThread is { } thread && thread != Thread.CurrentThread)
        {
            _ = thread.Join(TimeSpan.FromSeconds(2));
        }

        repeatThread = null;
    }

    private void RunRepeat()
    {
        Random random = new();
        while (running)
        {
            if (!SendOnce() && Marshal.GetLastWin32Error() is ErrorAccessDenied)
            {
                running = false;
                break;
            }

            Thread.Sleep(100 + random.Next(51));
        }
    }

    private bool SendOnce()
    {
        byte* inputs = stackalloc byte[InputSize * 2];
        new Span<byte>(inputs, InputSize * 2).Clear();

        if (kind is HutaoNativeHotKeyActionKind.MouseClickRepeatForever)
        {
            WriteUInt(inputs, 0, InputMouse);
            WriteUInt(inputs, 20, MouseLeftDown);
            WriteUInt(inputs, InputSize, InputMouse);
            WriteUInt(inputs, InputSize + 20, MouseLeftUp);
        }
        else
        {
            WriteUInt(inputs, 0, InputKeyboard);
            WriteUShort(inputs, 8, KeyFToggle);
            WriteUInt(inputs, InputSize, InputKeyboard);
            WriteUShort(inputs, InputSize + 8, KeyFToggle);
            WriteUInt(inputs, InputSize + 12, KeyEventKeyUp);
        }

        return SendInput(2, inputs, InputSize) is 2;
    }

    private static void WriteUInt(byte* destination, int offset, uint value)
    {
        *(uint*)(destination + offset) = value;
    }

    private static void WriteUShort(byte* destination, int offset, uint value)
    {
        *(ushort*)(destination + offset) = (ushort)value;
    }

    private static void EnsureWindow()
    {
        if (window is not 0)
        {
            return;
        }

        if (!classRegistered)
        {
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
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            finally
            {
                Marshal.FreeHGlobal(className);
            }

            classRegistered = true;
        }

        window = CreateWindowEx(0, WindowClassName, null, 0, 0, 0, 0, 0, HwndMessage, 0, 0, 0);
        if (window is 0)
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
    }

    private static nint WindowProc(nint hwnd, uint message, nint wParam, nint lParam)
    {
        if (message is WmHotKey)
        {
            if (BeforeSwitch?.Invoke() is true)
            {
                return 0;
            }

            HotKeyAction? action;
            lock (SyncRoot)
            {
                _ = Actions.TryGetValue((uint)wParam, out action);
            }

            if (action is { enabled: true } and { disposed: false })
            {
                action.Toggle();
            }

            return 0;
        }

        return DefWindowProc(hwnd, message, wParam, lParam);
    }

    private delegate nint WindowProcedure(nint hwnd, uint message, nint wParam, nint lParam);

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

    [LibraryImport("user32.dll", EntryPoint = "DefWindowProcW")]
    private static partial nint DefWindowProc(nint hwnd, uint message, nint wParam, nint lParam);

    [LibraryImport("user32.dll", EntryPoint = "RegisterHotKey", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool RegisterHotKey(nint hwnd, int id, HOT_KEY_MODIFIERS modifiers, uint virtualKey);

    [LibraryImport("user32.dll", EntryPoint = "UnregisterHotKey", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool UnregisterHotKey(nint hwnd, int id);

    [LibraryImport("user32.dll", EntryPoint = "SendInput")]
    private static partial uint SendInput(uint count, byte* inputs, int size);
}
