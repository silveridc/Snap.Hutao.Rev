// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Snap.Hutao.Rev.Migrations
{
    /// <inheritdoc />
    public partial class RestoreUidProfilePictures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE TABLE IF NOT EXISTS "uid_profile_pictures" (
                    "InnerId" TEXT NOT NULL CONSTRAINT "PK_uid_profile_pictures" PRIMARY KEY,
                    "Uid" TEXT NOT NULL,
                    "ProfilePictureId" INTEGER NOT NULL,
                    "AvatarId" INTEGER NOT NULL,
                    "CostumeId" INTEGER NOT NULL,
                    "RefreshTime" TEXT NOT NULL
                );
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "uid_profile_pictures");
        }
    }
}
