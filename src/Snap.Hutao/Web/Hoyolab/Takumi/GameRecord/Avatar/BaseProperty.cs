// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Intrinsic;

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.Avatar;

internal sealed class BaseProperty
{
    [JsonPropertyName("property_type")]
    public FightProperty PropertyType { get; set; }

    [JsonPropertyName("base")]
    public string Base { get; set; } = default!;

    [JsonPropertyName("add")]
    public string Add { get; set; } = default!;

    [JsonPropertyName("final")]
    public string Final { get; set; } = default!;
}