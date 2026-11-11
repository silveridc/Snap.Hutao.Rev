// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Text.Json.Annotation;

namespace Snap.Hutao.Rev.Web.Hutao;

internal sealed class HutaoPackageMirror
{
    [JsonPropertyName("url")]
    public string Url { get; set; } = default!;

    [JsonPropertyName("mirror_name")]
    public string MirrorName { get; set; } = default!;

    [JsonEnumHandling(JsonEnumHandling.String)]
    [JsonPropertyName("mirror_type")]
    public HutaoPackageMirrorType MirrorType { get; set; }
}
