using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hackathon.AuthService.Migrations
{
    /// <inheritdoc />
    public partial class ExtendRefreshTokenLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "307dc3b5-9a2d-4979-9ed5-b716a1674958");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "956c2eb1-04d4-4e39-bc31-cf778d36eb18");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "665dc513-f368-4efc-bf56-896d1da30cb5", null, "Admin", "ADMIN" },
                    { "a0ed447e-62fc-4f64-95f7-eb3dee213f5a", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "665dc513-f368-4efc-bf56-896d1da30cb5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0ed447e-62fc-4f64-95f7-eb3dee213f5a");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "307dc3b5-9a2d-4979-9ed5-b716a1674958", null, "Admin", "ADMIN" },
                    { "956c2eb1-04d4-4e39-bc31-cf778d36eb18", null, "User", "USER" }
                });
        }
    }
}
