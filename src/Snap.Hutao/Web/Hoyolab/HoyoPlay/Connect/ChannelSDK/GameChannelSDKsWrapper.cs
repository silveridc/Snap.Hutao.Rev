// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.HoyoPlay.Connect.ChannelSDK;

internal sealed class GameChannelSDKsWrapper
{
    [JsonPropertyName("game_channel_sdks")]
    public List<GameChannelSDK> GameChannelSDKs { get; set; } = default!;
}
