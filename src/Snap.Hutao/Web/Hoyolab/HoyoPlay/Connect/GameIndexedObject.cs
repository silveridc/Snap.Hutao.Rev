// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.HoyoPlay.Connect;

internal abstract class GameIndexedObject
{
    [JsonPropertyName("game")]
    public Game Game { get; set; } = default!;
}
