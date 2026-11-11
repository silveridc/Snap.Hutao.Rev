// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Text.RegularExpressions;

namespace Snap.Hutao.Rev.Web.Hutao;

// Represents the `data` object of the /api/v1/ip response envelope:
// { "retcode": 0, "message": "Success", "data": { "ip", "country", "province", "city", "isp" } }
internal sealed partial class IPInformation
{
    private const string Unknown = "Unknown";

    public static IPInformation Default { get; } = new()
    {
        Ip = Unknown,
        Country = Unknown,
        Province = Unknown,
        City = Unknown,
        Isp = Unknown,
    };

    [JsonPropertyName("ip")]
    public required string Ip { get; init; }

    [JsonPropertyName("country")]
    public string Country { get; init; } = string.Empty;

    [JsonPropertyName("province")]
    public string Province { get; init; } = string.Empty;

    [JsonPropertyName("city")]
    public string City { get; init; } = string.Empty;

    [JsonPropertyName("isp")]
    public string Isp { get; init; } = string.Empty;

    [GeneratedRegex(@"(\d+)\.(\d+)\.\d+\.\d+")]
    private static partial Regex IpRegex { get; }

    public override string ToString()
    {
        if (Ip is Unknown)
        {
            return SH.WebHutaoServiceUnAvailable;
        }

        string maskedIp = IpRegex.Replace(Ip, "$1.$2.*.*");
        string division = string.Join(' ', new[] { Country, Province, City, Isp }.Where(static segment => !string.IsNullOrEmpty(segment) && segment is not Unknown));
        return SH.FormatViewPageSettingDeviceIpDescription(maskedIp, string.IsNullOrEmpty(division) ? Unknown : division);
    }
}
