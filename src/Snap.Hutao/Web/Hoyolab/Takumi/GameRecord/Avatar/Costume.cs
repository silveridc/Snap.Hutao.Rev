// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Primitive;

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.Avatar;

internal sealed class Costume
{
    [JsonPropertyName("id")]
    public CostumeId Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;

    // Ignored field string icon
}