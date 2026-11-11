// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.PublicData.DeviceFp;

internal sealed class DeviceFpWrapper
{
    [JsonPropertyName("device_fp")]
    public string DeviceFp { get; set; } = default!;

    [JsonPropertyName("code")]
    public int Code { get; set; } = default!;

    [JsonPropertyName("message")]
    public string Message { get; set; } = default!;
}