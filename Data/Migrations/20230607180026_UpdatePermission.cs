using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class UpdatePermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermissionLabel",
                table: "Permission");

            migrationBuilder.RenameColumn(
                name: "PermissionName",
                table: "Permission",
                newName: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Name",
                table: "Permission",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Permission_Name",
                table: "Permission");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Permission",
                newName: "PermissionName");

            migrationBuilder.AddColumn<string>(
                name: "PermissionLabel",
                table: "Permission",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");
        }
    }
}
