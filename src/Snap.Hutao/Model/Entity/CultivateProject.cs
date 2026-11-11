// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Database.Abstraction;
using Snap.Hutao.Rev.UI.Xaml.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.Hutao.Rev.Model.Entity;

[Table("cultivate_projects")]
internal sealed partial class CultivateProject : ISelectable,
    IPropertyValuesProvider
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid InnerId { get; set; }

    public bool IsSelected { get; set; }

    public string Name { get; set; } = default!;

    public TimeSpan ServerTimeZoneOffset { get; set; }

    public static CultivateProject From(string name, in TimeSpan serverTimeOffset)
    {
        return new()
        {
            Name = name,
            ServerTimeZoneOffset = serverTimeOffset,
        };
    }
}