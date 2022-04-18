using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class AddLogTablefix3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedEntityValues",
                table: "LogCenters");

            migrationBuilder.RenameColumn(
                name: "UpdatedEntityValues",
                table: "LogCenters",
                newName: "EntityOldValues");

            migrationBuilder.AddColumn<string>(
                name: "EntityNewValues",
                table: "LogCenters",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntityNewValues",
                table: "LogCenters");

            migrationBuilder.RenameColumn(
                name: "EntityOldValues",
                table: "LogCenters",
                newName: "UpdatedEntityValues");

            migrationBuilder.AddColumn<string>(
                name: "CreatedEntityValues",
                table: "LogCenters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
