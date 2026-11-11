// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Intrinsic;
using Snap.Hutao.Rev.Model.Primitive;

namespace Snap.Hutao.Rev.Web.Enka.Model;

internal sealed class ShowAvatarInfo
{
    [JsonPropertyName("avatarId")]
    public AvatarId AvatarId { get; set; }

    [JsonPropertyName("level")]
    public Level Level { get; set; }

    [JsonPropertyName("energyType")]
    public ElementType EnergyType { get; set; }

    [JsonPropertyName("costumeId")]
    public CostumeId? CostumeId { get; set; }
}