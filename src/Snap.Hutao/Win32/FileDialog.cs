// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Runtime.InteropServices;

namespace Snap.Hutao.Rev.Win32;

// WinRT 选择器经由中完整性级别的 shell broker,提权进程调用会以 E_FAIL(0x80004005) 失败。
// 这里用进程内的 IFileDialog 兜底,提权下可用。
internal static unsafe partial class FileDialog
{
    private const uint ClsctxAll = 0x17;
    private const uint FosOverwritePrompt = 0x2;
    private const uint FosPickFolders = 0x20;
    private const uint FosForceFilesystem = 0x40;
    private const int ErrorCancelled = unchecked((int)0x800704C7);
    private const uint SigdnFilesysPath = 0x80058000;

    // IUnknown 0-2, IModalWindow::Show = 3, IFileDialog 从 4 起
    private const int ReleaseSlot = 2;
    private const int ShowSlot = 3;
    private const int SetFileTypesSlot = 4;
    private const int SetOptionsSlot = 9;
    private const int GetOptionsSlot = 10;
    private const int SetFileNameSlot = 15;
    private const int SetTitleSlot = 17;
    private const int GetResultSlot = 20;
    private const int SetDefaultExtensionSlot = 22;

    // IShellItem
    private const int GetDisplayNameSlot = 5;

    private static readonly Guid ClsidFileOpenDialog = new("DC1C5A9C-E88A-4DDE-A5A1-60F82A20AEF7");
    private static readonly Guid ClsidFileSaveDialog = new("C0B4E2F3-0536-4746-B210-3F4BE0E6BE65");
    private static readonly Guid IidFileDialog = new("42F8512D-ABDC-472B-B39C-F4EC9CBEBA2C");

    public static string? PickFolder(string? title, nint ownerWindow)
    {
        return Show(ClsidFileOpenDialog, ownerWindow, FosPickFolders, title, default, default, default);
    }

    public static string? PickFile(string? title, string? filterName, string? filterType, nint ownerWindow)
    {
        return Show(ClsidFileOpenDialog, ownerWindow, 0, title, default, default, BuildFilters(filterName, filterType));
    }

    public static string? SaveFile(string? title, string? defaultFileName, string? filterName, string? filterType, nint ownerWindow)
    {
        return Show(ClsidFileSaveDialog, ownerWindow, FosOverwritePrompt, title, defaultFileName, GetFirstExtension(filterType), BuildFilters(filterName, filterType));
    }

    private static string? Show(in Guid classId, nint ownerWindow, uint extraOptions, string? title, string? fileName, string? defaultExtension, (string Name, string Pattern)[]? filters)
    {
        int hr = CoCreateInstance(in classId, 0, ClsctxAll, in IidFileDialog, out nint dialog);
        if (hr < 0)
        {
            throw new COMException("Failed to create the file dialog.", hr);
        }

        nint filterSpecs = default;
        List<nint> filterStrings = [];
        nint shellItem = default;
        try
        {
            nint* vftbl = *(nint**)dialog;

            if (title is not null)
            {
                fixed (char* pTitle = title)
                {
                    ThrowIfFailed(((delegate* unmanaged[Stdcall]<nint, char*, int>)vftbl[SetTitleSlot])(dialog, pTitle), "Failed to set the dialog title.");
                }
            }

            if (fileName is not null)
            {
                fixed (char* pFileName = fileName)
                {
                    ThrowIfFailed(((delegate* unmanaged[Stdcall]<nint, char*, int>)vftbl[SetFileNameSlot])(dialog, pFileName), "Failed to set the file name.");
                }
            }

            if (defaultExtension is not null)
            {
                fixed (char* pExtension = defaultExtension)
                {
                    ThrowIfFailed(((delegate* unmanaged[Stdcall]<nint, char*, int>)vftbl[SetDefaultExtensionSlot])(dialog, pExtension), "Failed to set the default extension.");
                }
            }

            if (filters is not null)
            {
                filterSpecs = Marshal.AllocHGlobal(sizeof(FilterSpec) * filters.Length);
                FilterSpec* spec = (FilterSpec*)filterSpecs;
                for (int i = 0; i < filters.Length; i++)
                {
                    nint name = Marshal.StringToCoTaskMemUni(filters[i].Name);
                    nint pattern = Marshal.StringToCoTaskMemUni(filters[i].Pattern);
                    filterStrings.Add(name);
                    filterStrings.Add(pattern);
                    spec[i] = new() { Name = name, Pattern = pattern };
                }

                ThrowIfFailed(((delegate* unmanaged[Stdcall]<nint, uint, FilterSpec*, int>)vftbl[SetFileTypesSlot])(dialog, (uint)filters.Length, spec), "Failed to set the file types.");
            }

            uint options = 0;
            if (((delegate* unmanaged[Stdcall]<nint, uint*, int>)vftbl[GetOptionsSlot])(dialog, &options) >= 0)
            {
                ThrowIfFailed(((delegate* unmanaged[Stdcall]<nint, uint, int>)vftbl[SetOptionsSlot])(dialog, options | extraOptions | FosForceFilesystem), "Failed to set the dialog options.");
            }

            hr = ((delegate* unmanaged[Stdcall]<nint, nint, int>)vftbl[ShowSlot])(dialog, ownerWindow);
            if (hr == ErrorCancelled)
            {
                return default;
            }

            ThrowIfFailed(hr, "Failed to show the file dialog.");

            ThrowIfFailed(((delegate* unmanaged[Stdcall]<nint, nint*, int>)vftbl[GetResultSlot])(dialog, &shellItem), "Failed to get the dialog result.");

            nint displayName = default;
            ThrowIfFailed(((delegate* unmanaged[Stdcall]<nint, uint, nint*, int>)(*(nint**)shellItem)[GetDisplayNameSlot])(shellItem, SigdnFilesysPath, &displayName), "Failed to get the item path.");
            try
            {
                return Marshal.PtrToStringUni(displayName);
            }
            finally
            {
                Marshal.FreeCoTaskMem(displayName);
            }
        }
        finally
        {
            if (shellItem is not 0)
            {
                _ = ((delegate* unmanaged[Stdcall]<nint, uint>)(*(nint**)shellItem)[ReleaseSlot])(shellItem);
            }

            foreach (nint pointer in filterStrings)
            {
                Marshal.FreeCoTaskMem(pointer);
            }

            if (filterSpecs is not 0)
            {
                Marshal.FreeHGlobal(filterSpecs);
            }

            _ = ((delegate* unmanaged[Stdcall]<nint, uint>)(*(nint**)dialog)[ReleaseSlot])(dialog);
        }
    }

    private static (string Name, string Pattern)[] BuildFilters(string? filterName, string? filterType)
    {
        if (string.IsNullOrWhiteSpace(filterType))
        {
            return [("All files", "*")];
        }

        List<string> patterns = [];
        foreach (string pattern in filterType.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            string normalized = ToWildcardPattern(pattern);
            if (!patterns.Contains(normalized, StringComparer.OrdinalIgnoreCase))
            {
                patterns.Add(normalized);
            }
        }

        return patterns.Count is 0 ? [("All files", "*")] : [(filterName ?? "Files", string.Join(';', patterns))];
    }

    private static string ToWildcardPattern(string pattern)
    {
        int dot = pattern.LastIndexOf('.');
        return dot >= 0 ? "*" + pattern[dot..] : "*." + pattern;
    }

    private static string? GetFirstExtension(string? filterType)
    {
        string? first = filterType?.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).FirstOrDefault();
        if (first is null)
        {
            return default;
        }

        int dot = first.LastIndexOf('.');
        return dot >= 0 && dot < first.Length - 1 ? first[(dot + 1)..] : default;
    }

    private static void ThrowIfFailed(int hr, string message)
    {
        if (hr < 0)
        {
            throw new COMException(message, hr);
        }
    }

    [SuppressMessage("", "SA1307")]
    private struct FilterSpec
    {
        public nint Name;
        public nint Pattern;
    }

    [LibraryImport("ole32.dll")]
    private static partial int CoCreateInstance(in Guid classId, nint outer, uint context, in Guid interfaceId, out nint instance);
}