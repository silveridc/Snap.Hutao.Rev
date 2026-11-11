// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.Avatar;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.Hutao.Rev.Model.Entity;

[Table("avatar_infos")]
internal sealed class AvatarInfo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid InnerId { get; set; }

    public string Uid { get; set; } = default!;

    [MaybeNull]
    public DetailedCharacter Info2 { get; set; } = default!;

    [Obsolete]
    public DateTimeOffset RefreshTime { get; set; }

    public static AvatarInfo From(string uid, DetailedCharacter info)
    {
        return new() { Uid = uid, Info2 = info };
    }
}