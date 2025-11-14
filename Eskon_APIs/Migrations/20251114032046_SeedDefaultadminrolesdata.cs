using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Eskon_APIs.Migrations
{
    /// <inheritdoc />
    public partial class SeedDefaultadminrolesdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDisabled", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "92b75286-d8f8-4061-9995-e6e23ccdee94", "f51e5a91-bced-49c2-8b86-c2e170c0846c", false, false, "Admin", "ADMIN" },
                    { "9eaa03df-8e4f-4161-85de-0f6e5e30bfd4", "5ee6bc12-5cb0-4304-91e7-6a00744e042a", true, false, "Member", "MEMBER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsDisabled", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "dd026d4c-abe0-43ef-9ecd-5d99e737bc01", 0, "99d2bbc6-bc54-4248-a172-a77de3ae4430", "admin@Eskon.com", true, "Eskon", false, "Admin", false, null, "ADMIN@ESKON.COM", "ADMIN@ESKON.COM", "AQAAAAIAAYagAAAAEIW3NlZn9JYsa1r4A98wfj4CNoSIfdtdMYb7T3JlYn3ZZenM6GeinhFZqJVYCcTQ7A==", null, false, "55BF92C9EF0249CDA210D85D1A851BC9", false, "admin@Eskon.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "92b75286-d8f8-4061-9995-e6e23ccdee94", "dd026d4c-abe0-43ef-9ecd-5d99e737bc01" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9eaa03df-8e4f-4161-85de-0f6e5e30bfd4");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "92b75286-d8f8-4061-9995-e6e23ccdee94", "dd026d4c-abe0-43ef-9ecd-5d99e737bc01" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "92b75286-d8f8-4061-9995-e6e23ccdee94");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dd026d4c-abe0-43ef-9ecd-5d99e737bc01");
        }
    }
}
