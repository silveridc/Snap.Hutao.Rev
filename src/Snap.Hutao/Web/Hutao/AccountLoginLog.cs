// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hutao;

internal sealed class AccountLoginLogPage
{
    [JsonPropertyName("total")]
    public long Total { get; init; }

    [JsonPropertyName("items")]
    public List<AccountLoginLogEntry> Items { get; init; } = [];
}

internal sealed class AccountLoginLogEntry
{
    [JsonPropertyName("loginAt")]
    public DateTimeOffset LoginAt { get; init; }

    [JsonPropertyName("method")]
    public string Method { get; init; } = string.Empty;

    [JsonPropertyName("success")]
    public bool Success { get; init; }

    [JsonPropertyName("ip")]
    public string? Ip { get; init; }

    /// <summary>IP2Region 的四段：国家/省份/城市/运营商，缺段为 "0"，查不到为 null。</summary>
    [JsonPropertyName("ipRegion")]
    public string?[]? IpRegion { get; init; }

    [JsonPropertyName("deviceId")]
    public string? DeviceId { get; init; }

    [JsonPropertyName("deviceName")]
    public string? DeviceName { get; init; }

    [JsonPropertyName("operatingSystem")]
    public string? OperatingSystem { get; init; }
}
