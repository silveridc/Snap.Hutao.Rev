// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Snap.Hutao.Rev.Win32;

// VS 调试器下关闭一个 XAML 窗口会以 0xC000027B(STATUS_STOWED_EXCEPTION) 崩掉，正式包同一路径干净退出。
// 实测：Microsoft.ui.xaml.dll 两个构建（框架包 3.1.8.2608、自包含 3.3.0.0）里 0xC000027B 这个立即数
// 出现 0 次；combase.dll 全文只有 1 次，位于导出 RoFailFastWithErrorContextInternal2 内部，
// 它经 combase 自己的 kernel32 IAT 槽调用 RaiseFailFastException。所以这里是全进程唯一的引爆点，
// 只替换这一个槽：stowed 一类咽下，其余 fastfail 原样转给真函数，XAML 自身的 71 处 __fastfail 不受影响。
internal static unsafe partial class StowedFailFastPatch
{
    private const int StowedExceptionCode = unchecked((int)0xC000027B);
    private const uint PageReadWrite = 0x4;
    private const int PeHeaderOffset = 0x3C;
    private const int OptionalHeaderOffset = 24;
    private const int DataDirectoryOffset = 112;
    private const int ImportDirectoryIndex = 1;
    private const int ImportDescriptorSize = 20;
    private const int OriginalFirstThunkOffset = 0;
    private const int DescriptorNameOffset = 12;
    private const int FirstThunkOffset = 16;
    private const int ImportByNameHintSize = 2;
    private const int ThunkSize = 8;

    private static nint real;
    private static int swallowed;

    [Conditional("DEBUG")]
    public static void TryInstall()
    {
        if (!Debugger.IsAttached || real is not 0)
        {
            return;
        }

        // 用 LoadLibrary 而不是 GetModuleHandle：已加载时拿到的是同一个实例，
        // 早于 COM 初始化调用也能保证 combase 就位。
        nint module = LoadLibrary("combase.dll");
        nint slot = module is 0 ? 0 : FindImportSlot(module, "RaiseFailFastException");
        nint target = slot is 0 ? 0 : *(nint*)slot;
        if (target is 0)
        {
            Debug.WriteLine("[StowedFailFastPatch] 未找到 combase 的 RaiseFailFastException 导入槽，补丁未安装");
            return;
        }

        nint stub = (nint)(delegate* unmanaged[Stdcall]<nint, nint, uint, void>)&FilteredRaise;
        if (!VirtualProtect(slot, ThunkSize, PageReadWrite, out uint oldProtect))
        {
            Debug.WriteLine($"[StowedFailFastPatch] VirtualProtect 失败: {Marshal.GetLastPInvokeError()}");
            return;
        }

        try
        {
            real = target;
            *(nint*)slot = stub;
            Debug.WriteLine($"[StowedFailFastPatch] 已接管 combase+0x{slot - module:X}，原函数 0x{target:X16}");
        }
        finally
        {
            _ = VirtualProtect(slot, ThunkSize, oldProtect, out _);
        }
    }

    // 只按函数名匹配，不核对来源 DLL：combase 的 RaiseFailFastException 是从
    // api-ms-win-core-errorhandling-l1-1-2.dll 这个 apiset 导入的，写成 kernel32.dll 会找不到。
    private static nint FindImportSlot(nint module, string functionName)
    {
        byte* ntHeaders = (byte*)module + *(int*)(module + PeHeaderOffset);
        uint importDirectoryRva = *(uint*)(ntHeaders + OptionalHeaderOffset + DataDirectoryOffset + (ImportDirectoryIndex * 8));
        if (importDirectoryRva is 0)
        {
            return 0;
        }

        byte* descriptor = (byte*)module + (int)importDirectoryRva;
        while (*(uint*)(descriptor + DescriptorNameOffset) is not 0)
        {
            nint slot = FindSlotInDescriptor(module, descriptor, functionName);
            if (slot is not 0)
            {
                return slot;
            }

            descriptor += ImportDescriptorSize;
        }

        return 0;
    }

    private static nint FindSlotInDescriptor(nint module, byte* descriptor, string functionName)
    {
        // 名称表与原 IAT 同序；绑定过的模块名称表可能为空，此时退回 IAT 本身。
        int nameTableRva = *(int*)(descriptor + OriginalFirstThunkOffset);
        int addressTableRva = *(int*)(descriptor + FirstThunkOffset);
        byte* nameTable = (byte*)module + (nameTableRva is not 0 ? nameTableRva : addressTableRva);
        for (int index = 0; *(ulong*)(nameTable + (index * ThunkSize)) is not 0; index++)
        {
            ulong thunk = *(ulong*)(nameTable + (index * ThunkSize));

            // 最高位为 1 表示按序号导入，没有名字可比。
            if ((thunk & 0x8000_0000_0000_0000UL) is not 0)
            {
                continue;
            }

            string imported = PtrToAnsiString(module + (int)(thunk & 0x7FFF_FFFFUL) + ImportByNameHintSize);
            if (imported.Equals(functionName, StringComparison.Ordinal))
            {
                return module + addressTableRva + (index * ThunkSize);
            }
        }

        return 0;
    }

    private static string PtrToAnsiString(nint pointer)
    {
        return Marshal.PtrToStringAnsi(pointer) ?? string.Empty;
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvStdcall)])]
    private static void FilteredRaise(nint exceptionRecord, nint contextRecord, uint flags)
    {
        if (exceptionRecord is not 0 && Marshal.ReadInt32(exceptionRecord) is StowedExceptionCode)
        {
            Interlocked.Increment(ref swallowed);
            return;
        }

        ((delegate* unmanaged[Stdcall]<nint, nint, uint, void>)(void*)real)(exceptionRecord, contextRecord, flags);
    }

    [LibraryImport("kernel32.dll", EntryPoint = "LoadLibraryW", StringMarshalling = StringMarshalling.Utf16)]
    private static partial nint LoadLibrary(string fileName);

    [LibraryImport("kernel32.dll", EntryPoint = "VirtualProtect", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool VirtualProtect(nint address, nuint size, uint newProtect, out uint oldProtect);
}
