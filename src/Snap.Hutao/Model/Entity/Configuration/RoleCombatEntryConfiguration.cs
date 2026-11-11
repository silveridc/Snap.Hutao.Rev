// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.RoleCombat;

namespace Snap.Hutao.Rev.Model.Entity.Configuration;

internal sealed class RoleCombatEntryConfiguration : IEntityTypeConfiguration<RoleCombatEntry>
{
    public void Configure(EntityTypeBuilder<RoleCombatEntry> builder)
    {
        builder.Property(e => e.RoleCombatData)
            .HasColumnType(SqliteTypeNames.Text)
            .HasConversion<JsonTextValueConverter<RoleCombatData>>();
    }
}