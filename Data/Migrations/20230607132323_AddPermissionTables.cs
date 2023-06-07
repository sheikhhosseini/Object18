using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class AddPermissionTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PermissionActionId",
                table: "Permission",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PermissionAreaId",
                table: "Permission",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PermissionModuleId",
                table: "Permission",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "PermissionAction",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionAction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionArea",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionArea", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionModule",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionModule", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permission_PermissionActionId",
                table: "Permission",
                column: "PermissionActionId");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_PermissionAreaId",
                table: "Permission",
                column: "PermissionAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_PermissionModuleId",
                table: "Permission",
                column: "PermissionModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionAction_Name",
                table: "PermissionAction",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissionArea_Name",
                table: "PermissionArea",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissionModule_Name",
                table: "PermissionModule",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Permission_PermissionAction_PermissionActionId",
                table: "Permission",
                column: "PermissionActionId",
                principalTable: "PermissionAction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Permission_PermissionArea_PermissionAreaId",
                table: "Permission",
                column: "PermissionAreaId",
                principalTable: "PermissionArea",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Permission_PermissionModule_PermissionModuleId",
                table: "Permission",
                column: "PermissionModuleId",
                principalTable: "PermissionModule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permission_PermissionAction_PermissionActionId",
                table: "Permission");

            migrationBuilder.DropForeignKey(
                name: "FK_Permission_PermissionArea_PermissionAreaId",
                table: "Permission");

            migrationBuilder.DropForeignKey(
                name: "FK_Permission_PermissionModule_PermissionModuleId",
                table: "Permission");

            migrationBuilder.DropTable(
                name: "PermissionAction");

            migrationBuilder.DropTable(
                name: "PermissionArea");

            migrationBuilder.DropTable(
                name: "PermissionModule");

            migrationBuilder.DropIndex(
                name: "IX_Permission_PermissionActionId",
                table: "Permission");

            migrationBuilder.DropIndex(
                name: "IX_Permission_PermissionAreaId",
                table: "Permission");

            migrationBuilder.DropIndex(
                name: "IX_Permission_PermissionModuleId",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "PermissionActionId",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "PermissionAreaId",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "PermissionModuleId",
                table: "Permission");
        }
    }
}
