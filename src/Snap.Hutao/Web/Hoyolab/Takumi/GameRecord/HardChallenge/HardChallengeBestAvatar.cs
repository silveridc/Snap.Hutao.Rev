// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.
using Snap.Hutao.Rev.Model.Primitive;

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.HardChallenge;

internal sealed class HardChallengeBestAvatar
{
    [JsonPropertyName("avatar_id")]
    public required AvatarId AvatarId { get; init; }

    [JsonPropertyName("side_icon")]
    public required Uri SideIcon { get; init; }

    /// <summary>
    /// 实际上是总值或最值
    /// </summary>
    [JsonPropertyName("dps")]
    public required int Dps { get; init; }

    [JsonPropertyName("type")]
    public required HardChallengeBestAvatarType Type { get; init; }
}