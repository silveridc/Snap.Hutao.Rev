// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace Snap.Hutao.Rev.Win32;

// 原先由 Snap.Hutao.Native.dll 提供的注册表变化通知，改为 C# 轮询 RegNotifyChangeKeyValue。
internal sealed partial class RegistryNotification : IDisposable
{
    private const int RegNotifyChangeLastSet = 0x4;

    private readonly RegistryKey root;
    private readonly string subKey;

    private AutoResetEvent? stopEvent;
    private volatile bool disposed;

    public RegistryNotification(string path)
    {
        int separator = path.IndexOf('\\');
        string hive = separator < 0 ? path : path[..separator];
        subKey = separator < 0 ? string.Empty : path[(separator + 1)..];
        root = hive switch
        {
            "HKEY_CURRENT_USER" => Registry.CurrentUser,
            "HKEY_LOCAL_MACHINE" => Registry.LocalMachine,
            _ => throw new NotSupportedException($"Unsupported registry hive: {hive}"),
        };
    }

    public void Start(Action callback)
    {
        stopEvent = new(false);
        Thread thread = new(() => Watch(callback))
        {
            IsBackground = true,
            Name = "Snap.Hutao RegistryNotification",
        };

        thread.Start();
    }

    public void Dispose()
    {
        disposed = true;
        stopEvent?.Set();
        stopEvent?.Dispose();
        stopEvent = null;
    }

    private void Watch(Action callback)
    {
        using AutoResetEvent changeEvent = new(false);
        using RegistryKey? key = root.OpenSubKey(subKey);
        if (key is null || stopEvent is not { } stop)
        {
            return;
        }

        while (!disposed)
        {
            if (RegNotifyChangeKeyValue(key.Handle, true, RegNotifyChangeLastSet, changeEvent.SafeWaitHandle, true) is not 0)
            {
                return;
            }

            if (WaitHandle.WaitAny([changeEvent, stop]) is not 0)
            {
                return;
            }

            callback();
        }
    }

    [LibraryImport("advapi32.dll", EntryPoint = "RegNotifyChangeKeyValue", SetLastError = true)]
    private static partial int RegNotifyChangeKeyValue(SafeRegistryHandle key, [MarshalAs(UnmanagedType.Bool)] bool watchSubtree, int notifyFilter, SafeWaitHandle eventHandle, [MarshalAs(UnmanagedType.Bool)] bool asynchronous);
}
