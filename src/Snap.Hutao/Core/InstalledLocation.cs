// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.ExceptionService;
using System.IO;
using Windows.ApplicationModel;

namespace Snap.Hutao.Rev.Core;

internal static class InstalledLocation
{
    public static string GetAbsolutePath(string relativePath)
    {
        // 归一化为规范绝对路径；根目录按运行方式选择：
        // - 打包(MSIX)运行：包安装目录才是 Assets 等资源的真实根(AppContext.BaseDirectory 在自包含打包时会指向 win-x64 运行时目录,拿不到包内资源,导致 0x80070002)。
        // - 免安装运行：回退到可执行文件目录。
        return Path.GetFullPath(Path.Combine(GetRoot(), relativePath));
    }

    public static void CopyFileFromApplicationUri(string url, string path)
    {
        if (url.StartsWith("ms-appx:///", StringComparison.OrdinalIgnoreCase))
        {
            string sourcePath = GetAbsolutePath(url["ms-appx:///".Length..].Replace('/', Path.DirectorySeparatorChar));
            File.Copy(sourcePath, path, true);
            return;
        }

        throw HutaoException.NotSupported($"Unsupported application URI: {url}");
    }

    private static string GetRoot()
    {
        try
        {
            return Package.Current.InstalledLocation.Path;
        }
        catch
        {
            // 无包标识(免安装)时 Package.Current 抛异常，回退到可执行目录
            return AppContext.BaseDirectory;
        }
    }
}
