using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedRolesIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: ["Id", "ConcurrencyStamp", "Name", "NormalizedName"],
                values: new object[,]
                {
                    { "064dfa1d-e00e-4383-bb12-2e65d35a4579", "d993f8c9-991d-4671-86f9-86b769a1c601", "Admin", "ADMIN" },
                    { "37a3a701 - 684f - 4de0 - a6b9 - 369b570b3ec4", "899fc7d2-f5ab-49ef-90f7-cc7b32a76123", "Editor", "EDITOR" },
                    { "d993f8c9-991d-4671-86f9-86b769a1c601", "ecf1f447-76ba-4689-9abb-477549bf5b45", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "064dfa1d-e00e-4383-bb12-2e65d35a4579");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "37a3a701 - 684f - 4de0 - a6b9 - 369b570b3ec4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d993f8c9-991d-4671-86f9-86b769a1c601");
        }
    }
}
