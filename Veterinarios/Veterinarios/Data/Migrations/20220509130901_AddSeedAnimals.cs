using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Veterinarios.Data.Migrations
{
    public partial class AddSeedAnimals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Owners",
                columns: new[] { "Id", "Email", "NIF", "Name", "Sex" },
                values: new object[,]
                {
                    { 1, null, "813635582", "Luís Freitas", "M" },
                    { 2, null, "854613462", "Andreia Gomes", "F" },
                    { 3, null, "265368715", "Cristina Sousa", "F" },
                    { 4, null, "835623190", "Sónia Rosa", "F" }
                });

            migrationBuilder.InsertData(
                table: "Animals",
                columns: new[] { "Id", "BirthDate", "Breed", "Name", "OwnerFK", "Photo", "Species", "Weight" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pastor Alemão", "Bubi", 1, "animal1.jpg", "Cão", 24.0 },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Serra Estrela", "Pastor", 3, "animal2.jpg", "Cão", 50.0 },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Serra Estrela", "Tripé", 2, "animal3.jpg", "Cão", 4.0 },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lusitana", "Saltador", 3, "animal4.jpg", "Cavalo", 580.0 },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "siamês", "Tareco", 3, "animal5.jpg", "Gato", 1.0 },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Labrador", "Cusca", 2, "animal6.jpg", "Cão", 45.0 },
                    { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dobermann", "Morde Tudo", 4, "animal7.jpg", "Cão", 39.0 },
                    { 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rottweiler", "Forte", 2, "animal8.jpg", "Cão", 20.0 },
                    { 9, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mirandesa", "Castanho", 3, "animal9.jpg", "Vaca", 652.0 },
                    { 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Persa", "Saltitão", 1, "animal10.jpg", "Gato", 2.0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Owners",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Owners",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Owners",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Owners",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
