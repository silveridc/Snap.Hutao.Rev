// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core;
using System.IO;
using System.Runtime.InteropServices;

namespace Snap.Hutao.Rev.Service.Game.Island;

// 远线程注入由自建 C++ 组件（src/Snap.Hutao.Native）提供，随包放在 Resource/Native/Snap.Hutao.Native.Ext.dll。
internal static unsafe class DllInjectionUtilities
{
    private const string ExtFileName = "Snap.Hutao.Native.Ext.dll";
    private const string ExtResourceUri = "ms-appx:///Resource/Native/Snap.Hutao.Native.Ext.dll";
    private const string ExtPdbFileName = "Snap.Hutao.Native.Ext.pdb";
    private const string ExtPdbResourceUri = "ms-appx:///Resource/Native/Snap.Hutao.Native.Ext.pdb.dat";

    private static readonly object LoadLock = new();
    private static nint extModule;
    private static nint extInjectWithFunction;

    public static void InjectUsingRemoteThread(ReadOnlySpan<char> dllPath, ReadOnlySpan<char> functionName, int processId)
    {
        fixed (char* pDllPath = dllPath)
        {
            fixed (char* pFunctionName = functionName)
            {
                EnsureExtLoaded();
                delegate* unmanaged[Stdcall]<char*, char*, int, int> inject = (delegate* unmanaged[Stdcall]<char*, char*, int, int>)extInjectWithFunction;
                Marshal.ThrowExceptionForHR(inject(pDllPath, pFunctionName, processId));
            }
        }
    }

    private static void EnsureExtLoaded()
    {
        lock (LoadLock)
        {
            if (extModule is not 0)
            {
                return;
            }

            string path = Path.Combine(HutaoRuntime.DataDirectory, ExtFileName);
            InstalledLocation.CopyFileFromApplicationUri(ExtResourceUri, path);

            try
            {
                // 符号放在 DLL 旁边，VS 调试时能直接命中原生断点；旧布局里没有 PDB 也不影响注入
                InstalledLocation.CopyFileFromApplicationUri(ExtPdbResourceUri, Path.Combine(HutaoRuntime.DataDirectory, ExtPdbFileName));
            }
            catch (Exception)
            {
            }

            if (!NativeLibrary.TryLoad(path, out nint module))
            {
                throw new InvalidOperationException($"Failed to load module '{path}'.");
            }

            extInjectWithFunction = NativeLibrary.GetExport(module, "HutaoInjectUsingRemoteThreadWithFunction");
            extModule = module;
        }
    }
}
