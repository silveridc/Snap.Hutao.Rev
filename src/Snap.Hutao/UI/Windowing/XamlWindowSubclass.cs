// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Snap.Hutao.Rev.UI.Input;
using Snap.Hutao.Rev.UI.Windowing.Abstraction;
using Snap.Hutao.Rev.UI.Xaml.Media.Backdrop;
using Snap.Hutao.Rev.Win32;
using Snap.Hutao.Rev.Win32.System.SystemServices;
using Snap.Hutao.Rev.Win32.UI.Shell;
using System.Runtime.InteropServices;
using static Snap.Hutao.Rev.Win32.ConstValues;

namespace Snap.Hutao.Rev.UI.Windowing;

internal sealed partial class XamlWindowSubclass : IDisposable
{
    // 按 HWND 查状态而不是回调里解 GCHandle：窗口销毁阶段 GCHandle 可能已被 Dispose，
    // 在 [UnmanagedCallersOnly] 里解引用（或抛异常）会直接触发 CLR fail-fast（0xC0000602）。
    private static readonly Dictionary<nint, XamlWindowSubclass> States = [];

    private readonly WindowSubclass native;
    private readonly Window window;
    private readonly Lock syncRoot = new();
    private bool taskBarInitialized;

    public unsafe XamlWindowSubclass(Window window)
    {
        this.window = window;
        States[(nint)window.GetWindowHandle()] = this;
        native = new(window.GetWindowHandle(), OnSubclassProcedure);
    }

    public void Initialize()
    {
        native.Attach();
    }

    public void Dispose()
    {
        States.Remove((nint)window.GetWindowHandle());

        try
        {
            native.Detach();
        }
        catch (COMException)
        {
            // 0x80004005 E_FAIL
        }
    }

    public void SetTaskbarProgress(TBPFLAG state, ulong value, ulong maximum)
    {
        lock (syncRoot)
        {
            if (!taskBarInitialized)
            {
                native.InitializeTaskbarProgress();
                taskBarInitialized = true;
            }

            native.SetTaskbarProgress(state, value, maximum);
        }
    }

    /// <returns>Whether to call the original window procedure</returns>
    private static unsafe bool OnSubclassProcedure(nint hwnd, uint uMsg, nint wParam, nint lParam, out nint result)
    {
        result = 0;

        // 退出阶段或已经 detach 时必须继续调用原窗口过程：
        // 否则 XAML 收不到 WM_DESTROY/WM_NCDESTROY，关停时就会在后面解引用已销毁的窗口。
        if (XamlApplicationLifetime.Exiting || !States.TryGetValue(hwnd, out XamlWindowSubclass? state))
        {
            return true;
        }

        switch (uMsg)
        {
            case WM_NCRBUTTONDOWN:
            case WM_NCRBUTTONUP:
                return false;

            case WM_NCLBUTTONDBLCLK:
                {
                    if (state.window.AppWindow.Presenter is OverlappedPresenter { IsMaximizable: false })
                    {
                        return false;
                    }

                    break;
                }

            case WM_ERASEBKGND:
                {
                    if (state.window is IWindowNeedEraseBackground ||
                        state.window.SystemBackdrop is IBackdropNeedEraseBackground)
                    {
                        result = 1;
                        return false;
                    }

                    break;
                }

            case WM_MOUSEWHEEL:
                {
                    if (state.window is IXamlWindowMouseWheelHandler handler)
                    {
                        WPARAM2MOUSEWHEEL pWParam2 = *(WPARAM2MOUSEWHEEL*)&wParam;
                        LPARAM2MOUSEWHEEL pLParam2 = *(LPARAM2MOUSEWHEEL*)&lParam;
                        PointerPointProperties data = new(pWParam2.High, (MODIFIERKEYS_FLAGS)pWParam2.Low, pLParam2.Low, pLParam2.High);
                        handler.OnMouseWheel(in data);
                    }

                    break;
                }
        }

        return true;
    }

#pragma warning disable CA1823
#pragma warning disable CS0169
#pragma warning disable CS0649

    // ReSharper disable once InconsistentNaming
    private readonly struct WPARAM2MOUSEWHEEL
    {
        public readonly short Low;
        public readonly short High;
        private readonly int reserved;
    }

    // ReSharper disable once InconsistentNaming
    private readonly struct LPARAM2MOUSEWHEEL
    {
        public readonly short Low;
        public readonly short High;
        private readonly int reserved;
    }

#pragma warning restore CS0649
#pragma warning restore CS0169
#pragma warning restore CA1823
}