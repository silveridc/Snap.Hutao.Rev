// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Win32;
using Snap.Hutao.Rev.Win32.UI.WindowsAndMessaging;
using static Snap.Hutao.Rev.Win32.ConstValues;

namespace Snap.Hutao.Rev.UI.Input.LowLevel;

internal delegate void InputLowLevelKeyboardSourceEventHandler(LowLevelKeyEventArgs args);

internal static class InputLowLevelKeyboardSource
{
    public static event InputLowLevelKeyboardSourceEventHandler? KeyDown;

    public static event InputLowLevelKeyboardSourceEventHandler? KeyUp;

    public static event InputLowLevelKeyboardSourceEventHandler? SystemKeyDown;

    public static event InputLowLevelKeyboardSourceEventHandler? SystemKeyUp;

    [field: MaybeNull]
    private static LowLevelKeyboardHook Native
    {
        get => LazyInitializer.EnsureInitialized(ref field, static () => new());
    }

    public static unsafe void Initialize()
    {
        Native.Attach(ProcessLowLevelKeyboard);
    }

    public static unsafe void Uninitialize()
    {
        Native.Detach(ProcessLowLevelKeyboard);
    }

    private static bool ProcessLowLevelKeyboard(uint param, in KBDLLHOOKSTRUCT data)
    {
        LowLevelKeyEventArgs args = new(data);
        switch (param)
        {
            case WM_KEYDOWN:
                KeyDown?.Invoke(args);
                break;
            case WM_KEYUP:
                KeyUp?.Invoke(args);
                break;
            case WM_SYSKEYDOWN:
                SystemKeyDown?.Invoke(args);
                break;
            case WM_SYSKEYUP:
                SystemKeyUp?.Invoke(args);
                break;
        }

        return args.Handled;
    }
}