// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Primitive;

namespace Snap.Hutao.Rev.Web.Enka.Model;

internal sealed class ProfilePicture
{
    [JsonPropertyName("id")]
    public ProfilePictureId Id { get; set; }

    [JsonPropertyName("avatarId")]
    public AvatarId AvatarId { get; set; }

    [JsonPropertyName("costumeId")]
    public CostumeId CostumeId { get; set; }
}