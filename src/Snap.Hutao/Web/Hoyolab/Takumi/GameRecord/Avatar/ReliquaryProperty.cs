// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Intrinsic;

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.Avatar;

internal sealed class ReliquaryProperty
{
    [JsonPropertyName("property_type")]
    public FightProperty PropertyType { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; } = default!;

    [JsonPropertyName("times")]
    public uint Times { get; set; }
}