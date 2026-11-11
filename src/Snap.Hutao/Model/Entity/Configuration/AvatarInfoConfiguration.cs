// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.Avatar;

namespace Snap.Hutao.Rev.Model.Entity.Configuration;

internal sealed class AvatarInfoConfiguration : IEntityTypeConfiguration<AvatarInfo>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<AvatarInfo> builder)
    {
        builder.Property(e => e.Info2)
            .HasColumnType(SqliteTypeNames.Text)
            .HasConversion<JsonTextValueConverter<DetailedCharacter>>();
    }
}