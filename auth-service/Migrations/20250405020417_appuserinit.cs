using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace auth_service.Migrations
{
    /// <inheritdoc />
    public partial class appuserinit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8dd1639c-b2d0-46c0-bc99-6a42e1a6029d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d0762514-06d7-4eae-8864-c9d4180a47bd");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2d0f53ac-0c12-43b0-8f20-2c444877bcb4", null, "Admin", "ADMIN" },
                    { "9fa24b59-f39d-4b0d-8238-bc16028b2fb4", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2d0f53ac-0c12-43b0-8f20-2c444877bcb4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9fa24b59-f39d-4b0d-8238-bc16028b2fb4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8dd1639c-b2d0-46c0-bc99-6a42e1a6029d", null, "User", "USER" },
                    { "d0762514-06d7-4eae-8864-c9d4180a47bd", null, "Admin", "ADMIN" }
                });
        }
    }
}
