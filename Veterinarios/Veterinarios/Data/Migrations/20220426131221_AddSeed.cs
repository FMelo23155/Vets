using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Veterinarios.Data.Migrations
{
    public partial class AddSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProfessionalLicense",
                table: "Vets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Vets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Vets",
                columns: new[] { "Id", "Name", "Photo", "ProfessionalLicense" },
                values: new object[] { 1, "José Silva", "jose.jpg", "vet-8252" });

            migrationBuilder.InsertData(
                table: "Vets",
                columns: new[] { "Id", "Name", "Photo", "ProfessionalLicense" },
                values: new object[] { 2, "Maria Gomes", "maria.jpg", "vet-4143" });

            migrationBuilder.InsertData(
                table: "Vets",
                columns: new[] { "Id", "Name", "Photo", "ProfessionalLicense" },
                values: new object[] { 3, "Ricardo Pereira", "ricardo.jpg", "vet-6240" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Vets",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Vets",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Vets",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AlterColumn<string>(
                name: "ProfessionalLicense",
                table: "Vets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Vets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
