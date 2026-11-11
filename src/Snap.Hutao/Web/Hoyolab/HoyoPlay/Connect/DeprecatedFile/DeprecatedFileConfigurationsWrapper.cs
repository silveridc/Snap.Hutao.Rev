// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.HoyoPlay.Connect.DeprecatedFile;

internal sealed class DeprecatedFileConfigurationsWrapper
{
    [JsonPropertyName("deprecated_file_configs")]
    public List<DeprecatedFilesWrapper> DeprecatedFileConfigurations { get; set; } = default!;
}
