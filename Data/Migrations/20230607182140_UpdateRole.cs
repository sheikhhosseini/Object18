using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class UpdateRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RoleTitle",
                table: "Roles",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "RoleDescription",
                table: "Roles",
                newName: "Description");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Roles_Name",
                table: "Roles");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Roles",
                newName: "RoleTitle");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Roles",
                newName: "RoleDescription");
        }
    }
}
