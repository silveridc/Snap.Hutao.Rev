// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Database.Abstraction;
using Snap.Hutao.Rev.UI.Xaml.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.Hutao.Rev.Model.Entity;

[Table("gacha_archives")]
internal sealed partial class GachaArchive : ISelectable, IPropertyValuesProvider
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid InnerId { get; set; }

    public string Uid { get; set; } = default!;

    public bool IsSelected { get; set; }

    public static GachaArchive Create(string uid)
    {
        return new() { Uid = uid };
    }
}