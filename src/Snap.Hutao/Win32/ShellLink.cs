// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Runtime.InteropServices;

namespace Snap.Hutao.Rev.Win32;

// 原先由 Snap.Hutao.Native.dll 提供的 .lnk 快捷方式创建，改为 C# 实现。
internal static unsafe partial class ShellLink
{
    private const uint ClsctxAll = 0x17;
    private const int ShowCmdNormal = 1;
    private const int QueryInterfaceSlot = 0;
    private const int ReleaseSlot = 2;
    private const int SetArgumentsSlot = 11;
    private const int SetShowCmdSlot = 15;
    private const int SetIconLocationSlot = 17;
    private const int SetPathSlot = 20;
    private const int SaveSlot = 6;

    // 桌面被 OneDrive 接管、或被其它进程占用时写入会失败，这四种错误按创建成功处理。
    private const int AccessDenied = unchecked((int)0x80070005);
    private const int SharingViolation = unchecked((int)0x80070020);
    private const int UserMappedFile = unchecked((int)0x800704C8);
    private const int BadPathname = unchecked((int)0x80070522);

    private static readonly Guid ClsidShellLink = new("00021401-0000-0000-C000-000000000046");
    private static readonly Guid IidShellLinkW = new("000214F9-0000-0000-C000-000000000046");
    private static readonly Guid IidPersistFile = new("0000010B-0000-0000-C000-000000000046");

    public static void Create(string fileLocation, string arguments, string iconLocation, string fileName)
    {
        int hr = CoCreateInstance(in ClsidShellLink, 0, ClsctxAll, in IidShellLinkW, out nint shellLink);
        if (hr < 0)
        {
            throw new COMException("Failed to create ShellLink.", hr);
        }

        nint persistFile = default;
        try
        {
            nint* vftbl = *(nint**)shellLink;
            fixed (char* pArguments = arguments)
            {
                ThrowIfFailed(((delegate* unmanaged[Stdcall]<nint, char*, int>)vftbl[SetArgumentsSlot])(shellLink, pArguments), "Failed to set arguments.");
            }

            ThrowIfFailed(((delegate* unmanaged[Stdcall]<nint, int, int>)vftbl[SetShowCmdSlot])(shellLink, ShowCmdNormal), "Failed to set show command.");

            fixed (char* pIconLocation = iconLocation)
            {
                ThrowIfFailed(((delegate* unmanaged[Stdcall]<nint, char*, int, int>)vftbl[SetIconLocationSlot])(shellLink, pIconLocation, 0), "Failed to set icon location.");
            }

            fixed (char* pFileLocation = fileLocation)
            {
                ThrowIfFailed(((delegate* unmanaged[Stdcall]<nint, char*, int>)vftbl[SetPathSlot])(shellLink, pFileLocation), "Failed to set path.");
            }

            Guid persistFileIid = IidPersistFile;
            hr = ((delegate* unmanaged[Stdcall]<nint, Guid*, nint*, int>)vftbl[QueryInterfaceSlot])(shellLink, &persistFileIid, &persistFile);
            if (hr < 0)
            {
                throw new COMException("Failed to cast IShellLinkW to IPersistFile.", hr);
            }

            fixed (char* pFileName = fileName)
            {
                hr = ((delegate* unmanaged[Stdcall]<nint, char*, int, int>)(*(nint**)persistFile)[SaveSlot])(persistFile, pFileName, 0);
                if (hr < 0 && hr is not (AccessDenied or SharingViolation or UserMappedFile or BadPathname))
                {
                    throw new COMException("Failed to save the shell link.", hr);
                }
            }
        }
        finally
        {
            if (persistFile is not 0)
            {
                _ = ((delegate* unmanaged[Stdcall]<nint, uint>)(*(nint**)persistFile)[ReleaseSlot])(persistFile);
            }

            _ = ((delegate* unmanaged[Stdcall]<nint, uint>)(*(nint**)shellLink)[ReleaseSlot])(shellLink);
        }
    }

    private static void ThrowIfFailed(int hr, string message)
    {
        if (hr < 0)
        {
            throw new COMException(message, hr);
        }
    }

    [LibraryImport("ole32.dll")]
    private static partial int CoCreateInstance(in Guid classId, nint outer, uint context, in Guid interfaceId, out nint instance);
}
