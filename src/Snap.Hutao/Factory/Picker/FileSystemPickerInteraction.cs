// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core;
using Snap.Hutao.Rev.Core.IO;
using Snap.Hutao.Rev.Core.LifeCycle;
using Snap.Hutao.Rev.Win32;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace Snap.Hutao.Rev.Factory.Picker;

[Service(ServiceLifetime.Transient, typeof(IFileSystemPickerInteraction))]
internal sealed partial class FileSystemPickerInteraction : IFileSystemPickerInteraction
{
    private readonly ICurrentXamlWindowReference currentWindowReference;
    private readonly ITaskContext taskContext;

    [GeneratedConstructor]
    public partial FileSystemPickerInteraction(IServiceProvider serviceProvider);

    public async ValueTask<ValueResult<bool, ValueFile>> PickFileAsync(string? title, string? defaultFileName, string? filterName, string? filterType)
    {
        await taskContext.SwitchToMainThreadAsync();

        if (HutaoRuntime.IsProcessElevated)
        {
            string? picked = FileDialog.PickFile(title, filterName, filterType, currentWindowReference.WindowHandle);
            await taskContext.SwitchToBackgroundAsync();
            return picked is null ? new(false, default) : new(true, picked);
        }

        FileOpenPicker picker = new()
        {
            SuggestedStartLocation = PickerLocationId.ComputerFolder,
            ViewMode = PickerViewMode.List,
        };

        ApplyTitle(picker, title);
        AddFileTypes(picker.FileTypeFilter, filterType);
        InitializeWithWindow.Initialize(picker, currentWindowReference.WindowHandle);

        StorageFile? file = await picker.PickSingleFileAsync();
        await taskContext.SwitchToBackgroundAsync();
        return file is null ? new(false, default) : new(true, file.Path);
    }

    public async ValueTask<ValueResult<bool, ValueFile>> SaveFileAsync(string? title, string? defaultFileName, string? filterName, string? filterType)
    {
        await taskContext.SwitchToMainThreadAsync();

        if (HutaoRuntime.IsProcessElevated)
        {
            string? picked = FileDialog.SaveFile(title, defaultFileName, filterName, filterType, currentWindowReference.WindowHandle);
            await taskContext.SwitchToBackgroundAsync();
            return picked is null ? new(false, default) : new(true, picked);
        }

        FileSavePicker picker = new()
        {
            SuggestedStartLocation = PickerLocationId.ComputerFolder,
            SuggestedFileName = System.IO.Path.GetFileNameWithoutExtension(defaultFileName ?? string.Empty),
        };

        ApplyTitle(picker, title);
        IList<string> extensions = new List<string>();
        AddFileTypes(extensions, filterType);
        if (extensions.Count is 0)
        {
            extensions.Add(".json");
        }

        picker.FileTypeChoices.Add(filterName ?? "File", extensions);
        InitializeWithWindow.Initialize(picker, currentWindowReference.WindowHandle);

        StorageFile? file = await picker.PickSaveFileAsync();
        await taskContext.SwitchToBackgroundAsync();
        return file is null ? new(false, default) : new(true, file.Path);
    }

    public async ValueTask<ValueResult<bool, string?>> PickFolderAsync(string? title)
    {
        await taskContext.SwitchToMainThreadAsync();

        if (HutaoRuntime.IsProcessElevated)
        {
            string? picked = FileDialog.PickFolder(title, currentWindowReference.WindowHandle);
            await taskContext.SwitchToBackgroundAsync();
            return picked is null ? new(false, null) : new(true, picked);
        }

        FolderPicker picker = new()
        {
            SuggestedStartLocation = PickerLocationId.ComputerFolder,
        };

        ApplyTitle(picker, title);
        picker.FileTypeFilter.Add("*");
        InitializeWithWindow.Initialize(picker, currentWindowReference.WindowHandle);

        StorageFolder? folder = await picker.PickSingleFolderAsync();
        await taskContext.SwitchToBackgroundAsync();
        return folder is null ? new(false, null) : new(true, folder.Path);
    }

    private static void ApplyTitle(object picker, string? title)
    {
        if (string.IsNullOrEmpty(title))
        {
            return;
        }

        switch (picker)
        {
            case FileOpenPicker open:
                open.CommitButtonText = title;
                break;
            case FileSavePicker save:
                save.CommitButtonText = title;
                break;
            case FolderPicker folder:
                folder.CommitButtonText = title;
                break;
        }
    }

    private static void AddFileTypes(IList<string> fileTypeFilter, string? filterType)
    {
        if (string.IsNullOrEmpty(filterType))
        {
            return;
        }

        foreach (string pattern in filterType.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            // WinRT 只接受 ".ext" 单段扩展名,"*.exe"、"YuanShen.exe"、"exe" 都要归一成 ".exe"
            int dot = pattern.LastIndexOf('.');
            string extension = dot >= 0 ? pattern[dot..] : $".{pattern}";
            if (extension.Length is 1 || extension.AsSpan(1).ContainsAny('*', '?'))
            {
                continue;
            }

            if (!fileTypeFilter.Contains(extension))
            {
                fileTypeFilter.Add(extension);
            }
        }
    }
}
