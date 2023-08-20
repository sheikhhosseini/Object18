using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class UpdateUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_MobileNumber",
                table: "Users",
                column: "MobileNumber",
                unique: true,
                filter: "[MobileNumber] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_MobileNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Users");
        }
    }
}
