// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hutao;

[SuppressMessage("", "SA1516")]
internal sealed class HutaoPackageInformation
{
    [JsonPropertyName("version")]
    public Version Version { get; set; } = default!;

    [JsonPropertyName("validation")]
    public string Validation { get; set; } = default!;

    [JsonPropertyName("minimumVersion")]
    public Version? MinimumVersion { get; set; }

    [JsonPropertyName("fpsUnlockDisabled")]
    public bool FpsUnlockDisabled { get; set; }

    [JsonPropertyName("islandInjectionDisabled")]
    public bool IslandInjectionDisabled { get; set; }

    [JsonPropertyName("fpsUnlockDisabledReason")]
    public string? FpsUnlockDisabledReason { get; set; }

    [JsonPropertyName("islandInjectionDisabledReason")]
    public string? IslandInjectionDisabledReason { get; set; }

    [JsonPropertyName("mirrors")]
    public List<HutaoPackageMirror> Mirrors { get; set; } = default!;
}