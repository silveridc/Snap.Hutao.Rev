// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Snap.Hutao.Rev.Model.Entity.Configuration;

internal sealed class SpiralAbyssEntryConfiguration : IEntityTypeConfiguration<SpiralAbyssEntry>
{
    public void Configure(EntityTypeBuilder<SpiralAbyssEntry> builder)
    {
        builder.Property(e => e.SpiralAbyss)
            .HasColumnType(SqliteTypeNames.Text)
            .HasConversion<JsonTextValueConverter<Web.Hoyolab.Takumi.GameRecord.SpiralAbyss.SpiralAbyss>>();
    }
}