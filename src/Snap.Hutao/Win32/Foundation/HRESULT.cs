// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Win32.Foundation;

// ReSharper disable InconsistentNaming
[SuppressMessage("", "SA1310")]
internal readonly partial struct HRESULT
{
    public const int E_ASYNC_OPERATION_NOT_STARTED = unchecked((int)0x80000019);
    public const int E_FAIL = unchecked((int)0x80004005);
    public const int E_NOINTERFACE = unchecked((int)0x80004002);
    public const int E_UNEXPECTED = unchecked((int)0x8000FFFF);

#pragma warning disable CS0649
    public readonly int Value;
#pragma warning restore CS0649

    // 原先由 Snap.Hutao.Native.dll 的 HutaoHResultIsWin32 提供，改为 C# 实现。
    public static bool IsWin32(HRESULT hr, WIN32_ERROR error)
    {
        // Win32 错误码放进 HRESULT 时替换低 16 位，固定使用 0x8007 设施位。
        int value = (int)error;
        if (value > 0)
        {
            value = unchecked((int)0x80070000) | (value & 0xFFFF);
        }

        return hr.Value == value;
    }

    public static bool IsWin32(HRESULT hr, ReadOnlySpan<WIN32_ERROR> errors)
    {
        foreach (ref readonly WIN32_ERROR error in errors)
        {
            if (IsWin32(hr, error))
            {
                return true;
            }
        }

        return false;
    }

    // Win32Exception.HResult 恒为 E_FAIL，真实错误码只在 NativeErrorCode，故不能走 HResult 比较。
    public static bool IsWin32(Exception exception, WIN32_ERROR error)
    {
        return exception is Win32Exception win32
            ? win32.NativeErrorCode == (int)error
            : IsWin32(exception.HResult, error);
    }

    public static unsafe implicit operator int(HRESULT value)
    {
        return *(int*)&value;
    }

    public static unsafe implicit operator HRESULT(int value)
    {
        return *(HRESULT*)&value;
    }

    public override string ToString()
    {
        return $"0x{Value:X8}";
    }
}

#if DEBUG
[global::System.Diagnostics.DebuggerDisplay("{DebuggerDisplay}")]
internal readonly partial struct HRESULT
{
    private string DebuggerDisplay { get => ToString(); }
}
#endif