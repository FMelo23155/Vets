using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Veterinarios.Data.Migrations
{
    public partial class FkFromOwnerToAuthentication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "Owners",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Owners");
        }
    }
}
