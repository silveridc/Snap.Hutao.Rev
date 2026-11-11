// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Service.Game.FileSystem;
using Snap.Hutao.Rev.Web.Hoyolab.HoyoPlay.Connect.ChannelSDK;
using Snap.Hutao.Rev.Web.Hoyolab.HoyoPlay.Connect.DeprecatedFile;
using System.Net.Http;

namespace Snap.Hutao.Rev.Service.Game.Package;

internal readonly struct PackageConverterDeprecationContext
{
    public readonly HttpClient HttpClient;
    public readonly IGameFileSystemView GameFileSystem;
    public readonly GameChannelSDK? GameChannelSdk;
    public readonly DeprecatedFilesWrapper? DeprecatedFiles;

    public PackageConverterDeprecationContext(HttpClient httpClient, IGameFileSystemView gameFileSystem, GameChannelSDK? gameChannelSdk, DeprecatedFilesWrapper? deprecatedFiles)
    {
        HttpClient = httpClient;
        GameFileSystem = gameFileSystem;
        GameChannelSdk = gameChannelSdk;
        DeprecatedFiles = deprecatedFiles;
    }
}