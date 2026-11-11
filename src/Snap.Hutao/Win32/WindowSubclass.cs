// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Win32.UI.Shell;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Snap.Hutao.Rev.Win32;

internal delegate bool WindowSubclassProcedure(nint hwnd, uint message, nint wParam, nint lParam, out nint result);

// 原先由 Snap.Hutao.Native.dll 提供的窗口子类化与任务栏进度，改为 C# 实现。
// 子类化沿用原生 DLL 的 comctl32 多播子类（按序号 410/412/413 导入的三个函数），
// 不用 SetWindowLongPtr(GWLP_WNDPROC) 裸换窗口过程：裸换会把 XAML 与调试器已经钩住的
// 窗口过程压成 oldProc，而 Detach 发生在 window.Closed（也就是本回调自己的调用栈）里，
// 此时换回 oldProc 等于在同一条栈上把正在执行的链拆掉，关窗即以 0xC000027B 崩溃。
internal sealed unsafe partial class WindowSubclass : IDisposable
{
    private const uint ClsctxInprocServer = 0x1;
    private const nuint SubclassId = 1;

    private static readonly Dictionary<nint, WindowSubclass> States = [];
    private static readonly nint Procedure = (nint)(delegate* unmanaged[Stdcall]<nint, uint, nint, nint, nuint, nuint, nint>)&SubclassProc;

    private static readonly Guid ClsidTaskbarList = new("56FDF344-FD6D-11D0-958A-006097C9A090");
    private static readonly Guid IidTaskbarList3 = new("EA1AFB91-9E28-4B86-90E9-9E9F8A5EEFAF");

    private readonly nint hwnd;
    private readonly WindowSubclassProcedure callback;
    private bool attached;
    private nint taskbar;

    public WindowSubclass(nint hwnd, WindowSubclassProcedure callback)
    {
        this.hwnd = hwnd;
        this.callback = callback;
    }

    public void Attach()
    {
        if (attached)
        {
            return;
        }

        States[hwnd] = this;
        if (!SetWindowSubclass(hwnd, Procedure, SubclassId, 0))
        {
            States.Remove(hwnd);
            throw new InvalidOperationException($"SetWindowSubclass failed: {Marshal.GetLastWin32Error()}");
        }

        attached = true;
    }

    public void Detach()
    {
        States.Remove(hwnd);
        if (attached)
        {
            _ = RemoveWindowSubclass(hwnd, Procedure, SubclassId);
            attached = false;
        }
    }

    public void InitializeTaskbarProgress()
    {
        if (taskbar is not 0)
        {
            return;
        }

        _ = CoCreateInstance(in ClsidTaskbarList, 0, ClsctxInprocServer, in IidTaskbarList3, out nint created);
        if (created is 0)
        {
            throw new InvalidOperationException($"CoCreateInstance(TaskbarList) failed: {Marshal.GetLastWin32Error()}");
        }

        taskbar = created;
        _ = ((delegate* unmanaged[Stdcall]<nint, int>)(*(nint**)taskbar)[3])(taskbar); // HrInit
    }

    public void SetTaskbarProgress(TBPFLAG flags, ulong value, ulong maximum)
    {
        if (taskbar is 0)
        {
            return;
        }

        nint* vftbl = *(nint**)taskbar;
        _ = ((delegate* unmanaged[Stdcall]<nint, nint, int, int>)vftbl[10])(taskbar, hwnd, (int)flags); // SetProgressState
        if (flags is not TBPFLAG.TBPF_NOPROGRESS)
        {
            _ = ((delegate* unmanaged[Stdcall]<nint, nint, ulong, ulong, int>)vftbl[9])(taskbar, hwnd, value, maximum); // SetProgressValue
        }
    }

    public void Dispose()
    {
        Detach();

        if (taskbar is not 0)
        {
            _ = ((delegate* unmanaged[Stdcall]<nint, uint>)(*(nint**)taskbar)[2])(taskbar); // Release
            taskbar = 0;
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvStdcall)])]
    private static nint SubclassProc(nint hwnd, uint message, nint wParam, nint lParam, nuint idSubclass, nuint refData)
    {
        if (!States.TryGetValue(hwnd, out WindowSubclass? self))
        {
            return DefSubclassProc(hwnd, message, wParam, lParam);
        }

        try
        {
            if (!self.callback(hwnd, message, wParam, lParam, out nint result))
            {
                return result;
            }
        }
        catch
        {
            // 回调异常不能跨越本机边界（会触发 CLR fail-fast），退化为默认处理。
        }

        return DefSubclassProc(hwnd, message, wParam, lParam);
    }

    [LibraryImport("comctl32.dll", EntryPoint = "SetWindowSubclass", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetWindowSubclass(nint hwnd, nint procedure, nuint idSubclass, nuint refData);

    [LibraryImport("comctl32.dll", EntryPoint = "RemoveWindowSubclass", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool RemoveWindowSubclass(nint hwnd, nint procedure, nuint idSubclass);

    [LibraryImport("comctl32.dll", EntryPoint = "DefSubclassProc")]
    private static partial nint DefSubclassProc(nint hwnd, uint message, nint wParam, nint lParam);

    [LibraryImport("ole32.dll")]
    private static partial int CoCreateInstance(in Guid classId, nint outer, uint context, in Guid interfaceId, out nint instance);
}
