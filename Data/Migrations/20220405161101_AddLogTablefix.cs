using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class AddLogTablefix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OperationType",
                table: "LogCenters",
                newName: "Action");

            migrationBuilder.RenameColumn(
                name: "EntityValuesAfterOperation",
                table: "LogCenters",
                newName: "UpdatedEntityValues");

            migrationBuilder.RenameColumn(
                name: "EntityValues",
                table: "LogCenters",
                newName: "CreatedEntityValues");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedEntityValues",
                table: "LogCenters",
                newName: "EntityValuesAfterOperation");

            migrationBuilder.RenameColumn(
                name: "CreatedEntityValues",
                table: "LogCenters",
                newName: "EntityValues");

            migrationBuilder.RenameColumn(
                name: "Action",
                table: "LogCenters",
                newName: "OperationType");
        }
    }
}
