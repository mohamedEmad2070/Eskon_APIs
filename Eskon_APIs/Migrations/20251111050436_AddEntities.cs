using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eskon_APIs.Migrations
{
    /// <inheritdoc />
    public partial class AddEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Amenity",
                columns: table => new
                {
                    AmenityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmenityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amenity", x => x.AmenityId);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BuildingNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GeoLat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeoLng = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.LocationId);
                });

            migrationBuilder.CreateTable(
                name: "House",
                columns: table => new
                {
                    HouseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    PricePerMonth = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    NumberOfRooms = table.Column<int>(type: "int", nullable: false),
                    NumberOfBathrooms = table.Column<int>(type: "int", nullable: false),
                    Area = table.Column<double>(type: "float", nullable: false),
                    AverageRating = table.Column<double>(type: "float", nullable: true),
                    RatingCount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_House", x => x.HouseId);
                    table.ForeignKey(
                        name: "FK_House_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_House_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HouseAmenity",
                columns: table => new
                {
                    HouseId = table.Column<int>(type: "int", nullable: false),
                    AmenityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseAmenity", x => new { x.HouseId, x.AmenityId });
                    table.ForeignKey(
                        name: "FK_HouseAmenity_Amenity_AmenityId",
                        column: x => x.AmenityId,
                        principalTable: "Amenity",
                        principalColumn: "AmenityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HouseAmenity_House_HouseId",
                        column: x => x.HouseId,
                        principalTable: "House",
                        principalColumn: "HouseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaItem",
                columns: table => new
                {
                    MediaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    URL = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsCover = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HouseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaItem", x => x.MediaId);
                    table.ForeignKey(
                        name: "FK_MediaItem_House_HouseId",
                        column: x => x.HouseId,
                        principalTable: "House",
                        principalColumn: "HouseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Stars = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HouseId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_Review_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Review_House_HouseId",
                        column: x => x.HouseId,
                        principalTable: "House",
                        principalColumn: "HouseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SavedList",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HouseId = table.Column<int>(type: "int", nullable: false),
                    SavedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedList", x => new { x.UserId, x.HouseId });
                    table.ForeignKey(
                        name: "FK_SavedList_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedList_House_HouseId",
                        column: x => x.HouseId,
                        principalTable: "House",
                        principalColumn: "HouseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Amenity_Category_Name",
                table: "Amenity",
                columns: new[] { "Category", "AmenityName" });

            migrationBuilder.CreateIndex(
                name: "IX_Amenity_Name",
                table: "Amenity",
                column: "AmenityName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_House_LocationId",
                table: "House",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_House_LocationId_PricePerMonth",
                table: "House",
                columns: new[] { "LocationId", "PricePerMonth" });

            migrationBuilder.CreateIndex(
                name: "IX_House_OwnerId",
                table: "House",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_House_PricePerMonth",
                table: "House",
                column: "PricePerMonth");

            migrationBuilder.CreateIndex(
                name: "IX_HouseAmenity_AmenityId",
                table: "HouseAmenity",
                column: "AmenityId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_Address",
                table: "Location",
                columns: new[] { "City", "Street", "BuildingNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_Location_City_PostalCode",
                table: "Location",
                columns: new[] { "City", "PostalCode" });

            migrationBuilder.CreateIndex(
                name: "IX_MediaItem_HouseId",
                table: "MediaItem",
                column: "HouseId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaItem_HouseId_SortOrder",
                table: "MediaItem",
                columns: new[] { "HouseId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Review_HouseId",
                table: "Review",
                column: "HouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_UserId",
                table: "Review",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_UserId_HouseId",
                table: "Review",
                columns: new[] { "UserId", "HouseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SavedList_HouseId",
                table: "SavedList",
                column: "HouseId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedList_UserId",
                table: "SavedList",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HouseAmenity");

            migrationBuilder.DropTable(
                name: "MediaItem");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "SavedList");

            migrationBuilder.DropTable(
                name: "Amenity");

            migrationBuilder.DropTable(
                name: "House");

            migrationBuilder.DropTable(
                name: "Location");
        }
    }
}
