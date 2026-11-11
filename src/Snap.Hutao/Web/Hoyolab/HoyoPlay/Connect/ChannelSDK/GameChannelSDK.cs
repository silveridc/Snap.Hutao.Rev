// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.HoyoPlay.Connect.ChannelSDK;

internal sealed class GameChannelSDK : GameIndexedObject
{
    [JsonPropertyName("channel_sdk_pkg")]
    public PackageSegment ChannelSdkPackage { get; set; } = default!;

    [JsonPropertyName("pkg_version_file_name")]
    public string PackageVersionFileName { get; set; } = default!;

    [JsonPropertyName("version")]
    public string Version { get; set; } = default!;
}
