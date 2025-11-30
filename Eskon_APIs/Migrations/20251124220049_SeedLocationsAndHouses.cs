using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Eskon_APIs.Migrations
{
    /// <inheritdoc />
    public partial class SeedLocationsAndHouses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Amenity",
                columns: new[] { "AmenityId", "AmenityName", "Category" },
                values: new object[,]
                {
                    { 1, "WiFi", "General" },
                    { 2, "Air Conditioning", "Climate" },
                    { 3, "Swimming Pool", "Luxury" },
                    { 4, "Gym", "Luxury" },
                    { 5, "Parking", "General" }
                });

            migrationBuilder.InsertData(
                table: "Location",
                columns: new[] { "LocationId", "BuildingNumber", "City", "Country", "GeoLat", "GeoLng", "PostalCode", "Street" },
                values: new object[,]
                {
                    { 1, "15", "Cairo", "Egypt", "30.0444", "31.2357", "11511", "Talaat Harb St" },
                    { 2, "44", "Giza", "Egypt", "30.0131", "31.2089", "12311", "El Tahrir St" },
                    { 3, "102", "Alexandria", "Egypt", "31.2001", "29.9187", "21500", "Corniche Rd" }
                });

            migrationBuilder.InsertData(
                table: "House",
                columns: new[] { "HouseId", "Area", "AverageRating", "CreatedAt", "Description", "LocationId", "NumberOfBathrooms", "NumberOfRooms", "OwnerId", "PricePerMonth", "RatingCount", "Title" },
                values: new object[,]
                {
                    { 1, 120.0, null, new DateTime(2025, 10, 1, 15, 0, 0, 0, DateTimeKind.Local), "A spacious and modern apartment located in the heart of Cairo.", 1, 2, 3, "dd026d4c-abe0-43ef-9ecd-5d99e737bc01", 7500m, 0, "Modern Apartment in Downtown" },
                    { 2, 45.0, null, new DateTime(2025, 10, 2, 15, 0, 0, 0, DateTimeKind.Local), "Perfect for students. Compact, fully furnished studio.", 2, 1, 1, "dd026d4c-abe0-43ef-9ecd-5d99e737bc01", 3500m, 0, "Cozy Studio near University" },
                    { 3, 450.0, null, new DateTime(2025, 10, 3, 15, 0, 0, 0, DateTimeKind.Local), "High-end villa facing the sea with private garden.", 3, 4, 6, "dd026d4c-abe0-43ef-9ecd-5d99e737bc01", 25000m, 0, "Luxury Sea View Villa" },
                    { 4, 95.0, null, new DateTime(2025, 10, 4, 15, 0, 0, 0, DateTimeKind.Local), "Bright apartment with a large balcony and open kitchen plan.", 1, 1, 2, "dd026d4c-abe0-43ef-9ecd-5d99e737bc01", 6000m, 0, "Sunny 2-Bedroom Condo" },
                    { 5, 200.0, null, new DateTime(2025, 10, 5, 15, 0, 0, 0, DateTimeKind.Local), "Top floor penthouse offering panoramic views of the Nile.", 1, 3, 4, "dd026d4c-abe0-43ef-9ecd-5d99e737bc01", 12000m, 0, "Nile View Penthouse" },
                    { 6, 110.0, null, new DateTime(2025, 10, 6, 15, 0, 0, 0, DateTimeKind.Local), "Affordable 3-bedroom apartment in a quiet neighborhood.", 2, 2, 3, "dd026d4c-abe0-43ef-9ecd-5d99e737bc01", 4500m, 0, "Budget Family Home" },
                    { 7, 160.0, null, new DateTime(2025, 10, 7, 15, 0, 0, 0, DateTimeKind.Local), "Charming renovated townhouse with vintage architecture.", 1, 2, 4, "dd026d4c-abe0-43ef-9ecd-5d99e737bc01", 8000m, 0, "Historic Townhouse" },
                    { 8, 85.0, null, new DateTime(2025, 10, 8, 15, 0, 0, 0, DateTimeKind.Local), "Serviced apartment designed for business travelers.", 1, 2, 2, "dd026d4c-abe0-43ef-9ecd-5d99e737bc01", 9500m, 0, "Executive Suite" },
                    { 9, 140.0, null, new DateTime(2025, 10, 9, 15, 0, 0, 0, DateTimeKind.Local), "Relaxing home away from the city noise, featuring a backyard.", 2, 2, 3, "dd026d4c-abe0-43ef-9ecd-5d99e737bc01", 5500m, 0, "Suburban Garden House" },
                    { 10, 70.0, null, new DateTime(2025, 10, 10, 15, 0, 0, 0, DateTimeKind.Local), "Industrial style loft with high ceilings near the coast.", 3, 1, 1, "dd026d4c-abe0-43ef-9ecd-5d99e737bc01", 6800m, 0, "Compact Loft" }
                });

            migrationBuilder.InsertData(
                table: "SavedList",
                columns: new[] { "HouseId", "UserId", "SavedAt" },
                values: new object[] { 1, "dd026d4c-abe0-43ef-9ecd-5d99e737bc01", new DateTime(2025, 10, 11, 15, 0, 0, 0, DateTimeKind.Local) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "SavedList",
                keyColumns: new[] { "HouseId", "UserId" },
                keyValues: new object[] { 1, "dd026d4c-abe0-43ef-9ecd-5d99e737bc01" });

            migrationBuilder.DeleteData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Location",
                keyColumn: "LocationId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Location",
                keyColumn: "LocationId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Location",
                keyColumn: "LocationId",
                keyValue: 1);
        }
    }
}
