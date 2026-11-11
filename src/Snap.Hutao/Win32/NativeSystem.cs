// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace Snap.Hutao.Rev.Win32;

// 原先由 Snap.Hutao.Native.dll 提供的系统信息查询，改为纯 C# 实现。
internal static unsafe partial class NativeSystem
{
    private const uint FileShareReadWrite = 0x1 | 0x2;
    private const uint OpenExisting = 3;
    private const uint IoctlStorageGetDeviceNumber = 0x2D1080;
    private const uint IoctlStorageQueryProperty = 0x2D1400;
    private const int StorageDeviceSeekPenaltyProperty = 7;
    private const int PropertyStandardQuery = 0;
    private const uint DisplayDevicePrimary = 0x4;
    private const int EnumCurrentSettings = -1;

    private const uint MinimalBuild = 19045;
    private const uint MinimalRevision = 5371;

    public static WindowsVersion Version { get; } = GetVersion();

    public static bool IsCurrentWindowsVersionSupported()
    {
        WindowsVersion version = Version;
        if (version.Major != 10 || version.Minor != 0)
        {
            return version.Major > 10;
        }

        return version.Build > MinimalBuild || (version.Build == MinimalBuild && version.Revision >= MinimalRevision);
    }

    public static void ShowErrorMessage(string title, string message)
    {
        _ = MessageBox(0, message, title, 0x0 | 0x10 | 0x40000); // MB_OK | MB_ICONERROR | MB_TOPMOST
    }

    public static long GetDiskFreeSpace(string path)
    {
        return GetDiskFreeSpaceEx(path, out ulong freeBytesAvailable, out _, out _) ? (long)freeBytesAvailable : 0;
    }

    public static bool IsPathOnSolidStateDrive(string root)
    {
        if (!TryOpenDevice(string.Concat(@"\\.\", root.TrimEnd('\\')), out SafeFileHandle volume))
        {
            return false;
        }

        using (volume)
        {
            StorageDeviceNumber number = default;
            if (!DeviceIoControl(volume, IoctlStorageGetDeviceNumber, null, 0, &number, (uint)sizeof(StorageDeviceNumber), out _, 0))
            {
                return false;
            }

            if (!TryOpenDevice($@"\\.\PhysicalDrive{number.DeviceNumber}", out SafeFileHandle drive))
            {
                return false;
            }

            using (drive)
            {
                StoragePropertyQuery query = new() { PropertyId = StorageDeviceSeekPenaltyProperty, QueryType = PropertyStandardQuery };
                DeviceSeekPenaltyDescriptor descriptor = default;
                if (!DeviceIoControl(drive, IoctlStorageQueryProperty, &query, (uint)sizeof(StoragePropertyQuery), &descriptor, (uint)sizeof(DeviceSeekPenaltyDescriptor), out _, 0))
                {
                    return false;
                }

                return descriptor.IncursSeekPenalty is 0;
            }
        }
    }

    public static int GetPrimaryScreenVerticalRefreshRate()
    {
        DisplayDevice device = default;
        device.cb = (uint)sizeof(DisplayDevice);

        for (uint index = 0; EnumDisplayDevices(null, index, &device, 0); index++)
        {
            if ((device.StateFlags & DisplayDevicePrimary) is 0)
            {
                continue;
            }

            DevMode mode = default;
            mode.dmSize = (ushort)sizeof(DevMode);
            if (EnumDisplaySettings(device.DeviceName, EnumCurrentSettings, &mode))
            {
                return (int)mode.dmDisplayFrequency;
            }
        }

        return 0;
    }

    private static WindowsVersion GetVersion()
    {
        OsVersionInfo info = default;
        info.dwOSVersionInfoSize = (uint)sizeof(OsVersionInfo);
        return RtlGetVersion(ref info) is 0
            ? new(info.dwMajorVersion, info.dwMinorVersion, info.dwBuildNumber, GetUpdateBuildRevision())
            : default;
    }

    private static uint GetUpdateBuildRevision()
    {
        using Microsoft.Win32.RegistryKey? key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWAREMicrosoftWindows NTCurrentVersion");
        return key?.GetValue("UBR") is int revision ? (uint)revision : 0;
    }

    private static bool TryOpenDevice(string path, out SafeFileHandle handle)
    {
        handle = CreateFile(path, 0, FileShareReadWrite, 0, OpenExisting, 0, 0);
        return !handle.IsInvalid;
    }

    internal readonly struct WindowsVersion
    {
        public WindowsVersion(uint major, uint minor, uint build, uint revision)
        {
            Major = major;
            Minor = minor;
            Build = build;
            Revision = revision;
        }

        public uint Major { get; }

        public uint Minor { get; }

        public uint Build { get; }

        public uint Revision { get; }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Build}.{Revision}";
        }
    }

    [SuppressMessage("", "SA1307")]
    private struct OsVersionInfo
    {
        public uint dwOSVersionInfoSize;
        public uint dwMajorVersion;
        public uint dwMinorVersion;
        public uint dwBuildNumber;
        public uint dwPlatformId;
        public fixed char szCSDVersion[128];
        public ushort wServicePackMajor;
        public ushort wServicePackMinor;
        public ushort wSuiteMask;
        public byte wProductType;
        public byte wReserved;
    }

    [SuppressMessage("", "SA1307")]
    private struct StorageDeviceNumber
    {
        public uint DeviceType;
        public uint DeviceNumber;
        public uint PartitionNumber;
    }

    [SuppressMessage("", "SA1307")]
    private struct StoragePropertyQuery
    {
        public int PropertyId;
        public int QueryType;
        public fixed byte AdditionalParameters[1];
    }

    [SuppressMessage("", "SA1307")]
    private struct DeviceSeekPenaltyDescriptor
    {
        public uint Version;
        public uint Size;
        public byte IncursSeekPenalty;
    }

    [SuppressMessage("", "SA1307")]
    private struct DisplayDevice
    {
        public uint cb;
        public fixed char DeviceName[32];
        public fixed char DeviceString[128];
        public uint StateFlags;
        public fixed char DeviceID[128];
        public fixed char DeviceKey[128];
    }

    [SuppressMessage("", "SA1307")]
    private struct DevMode
    {
        public fixed char dmDeviceName[32];
        public ushort dmSpecVersion;
        public ushort dmDriverVersion;
        public ushort dmSize;
        public ushort dmDriverExtra;
        public uint dmFields;
        public int dmPositionX;
        public int dmPositionY;
        public uint dmDisplayOrientation;
        public uint dmDisplayFixedOutput;
        public short dmColor;
        public short dmDuplex;
        public short dmYResolution;
        public short dmTTOption;
        public short dmCollate;
        public fixed char dmFormName[32];
        public ushort dmLogPixels;
        public uint dmBitsPerPel;
        public uint dmPelsWidth;
        public uint dmPelsHeight;
        public uint dmDisplayFlags;
        public uint dmDisplayFrequency;
        public fixed byte dmRest[128];
    }

    [LibraryImport("user32.dll", EntryPoint = "MessageBoxW", StringMarshalling = StringMarshalling.Utf16)]
    private static partial int MessageBox(nint hwnd, string text, string caption, uint type);

    [LibraryImport("kernel32.dll", EntryPoint = "GetDiskFreeSpaceExW", StringMarshalling = StringMarshalling.Utf16, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetDiskFreeSpaceEx(string directoryName, out ulong freeBytesAvailableToCaller, out ulong totalNumberOfBytes, out ulong totalNumberOfFreeBytes);

    [LibraryImport("kernel32.dll", EntryPoint = "CreateFileW", StringMarshalling = StringMarshalling.Utf16, SetLastError = true)]
    private static partial SafeFileHandle CreateFile(string fileName, uint desiredAccess, uint shareMode, nint securityAttributes, uint creationDisposition, uint flagsAndAttributes, nint templateFile);

    [LibraryImport("kernel32.dll", EntryPoint = "DeviceIoControl", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool DeviceIoControl(SafeFileHandle device, uint controlCode, void* inBuffer, uint inBufferSize, void* outBuffer, uint outBufferSize, out uint bytesReturned, nint overlapped);

    [LibraryImport("ntdll.dll", EntryPoint = "RtlGetVersion")]
    private static partial int RtlGetVersion(ref OsVersionInfo versionInfo);

    [LibraryImport("user32.dll", EntryPoint = "EnumDisplayDevicesW", StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool EnumDisplayDevices(string? device, uint devNum, DisplayDevice* displayDevice, uint flags);

    [LibraryImport("user32.dll", EntryPoint = "EnumDisplaySettingsW", StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool EnumDisplaySettings(char* deviceName, int modeNum, DevMode* devMode);
}
