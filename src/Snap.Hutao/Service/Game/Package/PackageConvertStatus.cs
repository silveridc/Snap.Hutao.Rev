// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using CommunityToolkit.Common;

namespace Snap.Hutao.Rev.Service.Game.Package;

internal sealed class PackageConvertStatus
{
    public PackageConvertStatus(string name)
    {
        Name = name;
    }

    public PackageConvertStatus(string name, long bytesRead, long totalBytes)
    {
        Percent = (double)bytesRead / totalBytes;
        Name = name;
        Description = $"{Converters.ToFileSizeString(bytesRead)}/{Converters.ToFileSizeString(totalBytes)}";
    }

    public string Name { get; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; } = default!;

    /// <summary>
    /// 进度
    /// </summary>
    public double Percent { get; } = -1;

    /// <summary>
    /// 是否有进度
    /// </summary>
    public bool IsIndeterminate { get => Percent < 0; }
}