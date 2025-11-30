using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection;

namespace Eskon_APIs.Persistance;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
{
    public DbSet<House> House { get; set; }
    public DbSet<SavedList> SavedList { get; set; }
    public DbSet<Amenity> Amenity { get; set; }
    public DbSet<Location> Location { get; set; }
    public DbSet<MediaItem> MediaItem { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);

        // Define Composite Key for SavedList (if not defined in configurations)
        builder.Entity<SavedList>()
               .HasKey(s => new { s.UserId, s.HouseId });

        // ==========================================
        // SEEDING DATA
        // ==========================================

        string ownerId = "dd026d4c-abe0-43ef-9ecd-5d99e737bc01";

        // 1. Seed Locations (Updated to match your new class)
        builder.Entity<Location>().HasData(
            new Location
            {
                LocationId = 1,
                Country = "Egypt",
                City = "Cairo",
                Street = "Talaat Harb St",
                BuildingNumber = "15",
                PostalCode = "11511",
                GeoLat = "30.0444",
                GeoLng = "31.2357"
            },
            new Location
            {
                LocationId = 2,
                Country = "Egypt",
                City = "Giza",
                Street = "El Tahrir St",
                BuildingNumber = "44",
                PostalCode = "12311",
                GeoLat = "30.0131",
                GeoLng = "31.2089"
            },
            new Location
            {
                LocationId = 3,
                Country = "Egypt",
                City = "Alexandria",
                Street = "Corniche Rd",
                BuildingNumber = "102",
                PostalCode = "21500",
                GeoLat = "31.2001",
                GeoLng = "29.9187"
            }
        );

        // 2. Seed Amenities (Optional but recommended)
        builder.Entity<Amenity>().HasData(
            new Amenity { AmenityId = 1, AmenityName = "WiFi", Category = "General" },
            new Amenity { AmenityId = 2, AmenityName = "Air Conditioning", Category = "Climate" },
            new Amenity { AmenityId = 3, AmenityName = "Swimming Pool", Category = "Luxury" },
            new Amenity { AmenityId = 4, AmenityName = "Gym", Category = "Luxury" },
            new Amenity { AmenityId = 5, AmenityName = "Parking", Category = "General" }
        );

        // 3. Seed 10 Houses
        builder.Entity<House>().HasData(
            new House
            {
                HouseId = 1,
                Title = "Modern Apartment in Downtown",
                Description = "A spacious and modern apartment located in the heart of Cairo.",
                PricePerMonth = 7500m,
                NumberOfRooms = 3,
                NumberOfBathrooms = 2,
                Area = 120,
                LocationId = 1, // Cairo
                OwnerId = ownerId,
                CreatedAt = DateTime.Parse("2025-10-01T12:00:00Z")
            },
            new House
            {
                HouseId = 2,
                Title = "Cozy Studio near University",
                Description = "Perfect for students. Compact, fully furnished studio.",
                PricePerMonth = 3500m,
                NumberOfRooms = 1,
                NumberOfBathrooms = 1,
                Area = 45,
                LocationId = 2, // Giza
                OwnerId = ownerId,
                CreatedAt = DateTime.Parse("2025-10-02T12:00:00Z")
            },
            new House
            {
                HouseId = 3,
                Title = "Luxury Sea View Villa",
                Description = "High-end villa facing the sea with private garden.",
                PricePerMonth = 25000m,
                NumberOfRooms = 6,
                NumberOfBathrooms = 4,
                Area = 450,
                LocationId = 3, // Alexandria
                OwnerId = ownerId,
                CreatedAt = DateTime.Parse("2025-10-03T12:00:00Z")
            },
            new House
            {
                HouseId = 4,
                Title = "Sunny 2-Bedroom Condo",
                Description = "Bright apartment with a large balcony and open kitchen plan.",
                PricePerMonth = 6000m,
                NumberOfRooms = 2,
                NumberOfBathrooms = 1,
                Area = 95,
                LocationId = 1, // Cairo
                OwnerId = ownerId,
                CreatedAt = DateTime.Parse("2025-10-04T12:00:00Z")
            },
            new House
            {
                HouseId = 5,
                Title = "Nile View Penthouse",
                Description = "Top floor penthouse offering panoramic views of the Nile.",
                PricePerMonth = 12000m,
                NumberOfRooms = 4,
                NumberOfBathrooms = 3,
                Area = 200,
                LocationId = 1, // Cairo
                OwnerId = ownerId,
                CreatedAt = DateTime.Parse("2025-10-05T12:00:00Z")
            },
            new House
            {
                HouseId = 6,
                Title = "Budget Family Home",
                Description = "Affordable 3-bedroom apartment in a quiet neighborhood.",
                PricePerMonth = 4500m,
                NumberOfRooms = 3,
                NumberOfBathrooms = 2,
                Area = 110,
                LocationId = 2, // Giza
                OwnerId = ownerId,
                CreatedAt = DateTime.Parse("2025-10-06T12:00:00Z")
            },
            new House
            {
                HouseId = 7,
                Title = "Historic Townhouse",
                Description = "Charming renovated townhouse with vintage architecture.",
                PricePerMonth = 8000m,
                NumberOfRooms = 4,
                NumberOfBathrooms = 2,
                Area = 160,
                LocationId = 1, // Cairo
                OwnerId = ownerId,
                CreatedAt = DateTime.Parse("2025-10-07T12:00:00Z")
            },
            new House
            {
                HouseId = 8,
                Title = "Executive Suite",
                Description = "Serviced apartment designed for business travelers.",
                PricePerMonth = 9500m,
                NumberOfRooms = 2,
                NumberOfBathrooms = 2,
                Area = 85,
                LocationId = 1, // Cairo
                OwnerId = ownerId,
                CreatedAt = DateTime.Parse("2025-10-08T12:00:00Z")
            },
            new House
            {
                HouseId = 9,
                Title = "Suburban Garden House",
                Description = "Relaxing home away from the city noise, featuring a backyard.",
                PricePerMonth = 5500m,
                NumberOfRooms = 3,
                NumberOfBathrooms = 2,
                Area = 140,
                LocationId = 2, // Giza
                OwnerId = ownerId,
                CreatedAt = DateTime.Parse("2025-10-09T12:00:00Z")
            },
            new House
            {
                HouseId = 10,
                Title = "Compact Loft",
                Description = "Industrial style loft with high ceilings near the coast.",
                PricePerMonth = 6800m,
                NumberOfRooms = 1,
                NumberOfBathrooms = 1,
                Area = 70,
                LocationId = 3, // Alexandria
                OwnerId = ownerId,
                CreatedAt = DateTime.Parse("2025-10-10T12:00:00Z")
            }
        );

        // 4. Seed SavedList (Optional - shows this user saved house #1)
        builder.Entity<SavedList>().HasData(
            new SavedList
            {
                UserId = ownerId,
                HouseId = 1,
                SavedAt = DateTime.Parse("2025-10-11T12:00:00Z")
            }
        );
    }
}