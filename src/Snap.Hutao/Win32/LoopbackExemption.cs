// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Runtime.InteropServices;

namespace Snap.Hutao.Rev.Win32;

// 原先由 Snap.Hutao.Native.dll 提供的 AppContainer 回环豁免读写，改为 C# 实现。
internal static unsafe partial class LoopbackExemption
{
    private const uint EnumAppContainersFlags = 2;
    private const uint EnumNotReady = 1780;
    private const uint RpcServerUnavailable = 1753;
    private const int FirewallRpcUnavailable = unchecked((int)0x800706D9);
    private const uint ConfigAlreadyUpToDate = 0x80320006;
    private const int NetFwProfile2Public = 4;
    private const short VariantTrue = unchecked((short)0xFFFF);
    private const uint ClsctxInprocServer = 0x1;
    private const int GetFirewallEnabledSlot = 8;
    private const int ReleaseSlot = 2;

    private static readonly Guid ClsidNetFwPolicy2 = new("E2B3C97F-6AE1-41AC-817A-F6F92166D7DD");
    private static readonly Guid IidNetFwPolicy2 = new("98325047-C671-4174-8D81-DEFCD3F03186");

    public static bool IsPublicFirewallEnabled()
    {
        int hr = CoCreateInstance(in ClsidNetFwPolicy2, 0, ClsctxInprocServer, in IidNetFwPolicy2, out nint policy);
        if (hr < 0)
        {
            Marshal.ThrowExceptionForHR(hr);
        }

        try
        {
            short enabled = 0;
            nint* vftbl = *(nint**)policy;
            hr = ((delegate* unmanaged[Stdcall]<nint, int, short*, int>)vftbl[GetFirewallEnabledSlot])(policy, NetFwProfile2Public, &enabled);
            return hr switch
            {
                // mpsvc 未运行时防火墙状态不可知，按未启用处理。
                FirewallRpcUnavailable => false,
                < 0 => throw new COMException("Failed to check is firewall enabled.", hr),
                _ => enabled is VariantTrue,
            };
        }
        finally
        {
            _ = ((delegate* unmanaged[Stdcall]<nint, uint>)(*(nint**)policy)[ReleaseSlot])(policy);
        }
    }

    public static (bool Enabled, string? Sid) IsEnabled(string familyName)
    {
        uint error = NetworkIsolationEnumAppContainers(EnumAppContainersFlags, out uint count, out nint entries);
        if (error is EnumNotReady or RpcServerUnavailable)
        {
            return (false, null);
        }

        if (error is not 0)
        {
            throw new InvalidOperationException($"Failed to enum app containers: {error}");
        }

        try
        {
            string? sid = FindAppContainerSid((AppContainer*)entries, count, familyName);
            return sid is null ? (false, null) : (IsExempted(sid), sid);
        }
        finally
        {
            _ = NetworkIsolationFreeAppContainers(entries);
        }
    }

    public static void Enable(string sid)
    {
        if (!ConvertStringSidToSid(sid, out nint owner))
        {
            throw new InvalidOperationException($"Failed to convert string to sid: {Marshal.GetLastPInvokeError()}");
        }

        try
        {
            uint error = NetworkIsolationGetAppContainerConfig(out uint count, out nint config);
            if (error is not 0)
            {
                throw new InvalidOperationException($"Failed to get app container config: {error}");
            }

            // 豁免表是整体覆盖写入的，所以要连现有项一起提交，SID 追加在末尾。
            uint newSize = (count + 1) * (uint)sizeof(SidAndAttributes);
            nint buffer = (nint)NativeMemory.Alloc(newSize);
            try
            {
                Buffer.MemoryCopy((void*)config, (void*)buffer, newSize, count * (uint)sizeof(SidAndAttributes));
                ((SidAndAttributes*)buffer)[count] = new SidAndAttributes { Sid = owner, Attributes = 0 };
                error = NetworkIsolationSetAppContainerConfig(buffer, count + 1);
                if (error is not 0 and not ConfigAlreadyUpToDate)
                {
                    throw new InvalidOperationException($"Failed to set app container config: {error}");
                }
            }
            finally
            {
                NativeMemory.Free((void*)buffer);
                _ = LocalFree(config);
            }
        }
        finally
        {
            _ = LocalFree(owner);
        }
    }

    private static string? FindAppContainerSid(AppContainer* table, uint count, string familyName)
    {
        for (uint i = 0; i < count; i++)
        {
            if (string.Equals(new string((char*)table[i].AppContainerName), familyName, StringComparison.Ordinal))
            {
                return SidToString(table[i].AppContainerSid);
            }
        }

        return null;
    }

    private static bool IsExempted(string sid)
    {
        uint error = NetworkIsolationGetAppContainerConfig(out uint count, out nint config);
        if (error is not 0)
        {
            throw new InvalidOperationException($"Failed to get app container config: {error}");
        }

        try
        {
            SidAndAttributes* table = (SidAndAttributes*)config;
            for (uint i = 0; i < count; i++)
            {
                if (string.Equals(SidToString(table[i].Sid), sid, StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }
        finally
        {
            _ = LocalFree(config);
        }
    }

    private static string SidToString(nint sid)
    {
        if (!ConvertSidToStringSid(sid, out nint converted))
        {
            throw new InvalidOperationException($"Failed to convert sid to string: {Marshal.GetLastPInvokeError()}");
        }

        try
        {
            return new string((char*)converted);
        }
        finally
        {
            _ = LocalFree(converted);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct AppContainer
    {
        public nint AppContainerSid;
        public nint UserSid;
        public nint AppContainerName;
        public nint DisplayName;
        public nint Description;
        public nuint CapabilityCount;
        public nint Capabilities;
        public nuint BinaryCount;
        public nint Binaries;
        public nint WorkingDirectory;
        public nint PackageFullName;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct SidAndAttributes
    {
        public nint Sid;
        public uint Attributes;
    }

    [LibraryImport("api-ms-win-net-isolation-l1-1-0.dll", EntryPoint = "NetworkIsolationEnumAppContainers")]
    private static partial uint NetworkIsolationEnumAppContainers(uint flags, out uint count, out nint appContainers);

    [LibraryImport("api-ms-win-net-isolation-l1-1-0.dll", EntryPoint = "NetworkIsolationFreeAppContainers")]
    private static partial uint NetworkIsolationFreeAppContainers(nint appContainers);

    [LibraryImport("api-ms-win-net-isolation-l1-1-0.dll", EntryPoint = "NetworkIsolationGetAppContainerConfig")]
    private static partial uint NetworkIsolationGetAppContainerConfig(out uint count, out nint sids);

    [LibraryImport("api-ms-win-net-isolation-l1-1-0.dll", EntryPoint = "NetworkIsolationSetAppContainerConfig")]
    private static partial uint NetworkIsolationSetAppContainerConfig(nint sids, uint count);

    [LibraryImport("advapi32.dll", EntryPoint = "ConvertSidToStringSidW", StringMarshalling = StringMarshalling.Utf16, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool ConvertSidToStringSid(nint sid, out nint stringSid);

    [LibraryImport("advapi32.dll", EntryPoint = "ConvertStringSidToSidW", StringMarshalling = StringMarshalling.Utf16, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool ConvertStringSidToSid(string stringSid, out nint sid);

    [LibraryImport("kernel32.dll")]
    private static partial nint LocalFree(nint memory);

    [LibraryImport("ole32.dll")]
    private static partial int CoCreateInstance(in Guid classId, nint outer, uint context, in Guid interfaceId, out nint instance);
}
