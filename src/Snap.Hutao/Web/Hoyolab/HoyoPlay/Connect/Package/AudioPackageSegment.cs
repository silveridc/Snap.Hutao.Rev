// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.HoyoPlay.Connect.Package;

internal sealed class AudioPackageSegment : PackageSegment
{
    [JsonPropertyName("language")]
    public string Language { get; set; } = default!;
}
