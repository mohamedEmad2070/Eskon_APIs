
namespace Eskon_APIs.Persistance.EntitiesConfigurations;

public class HouseAmenityConfiguration : IEntityTypeConfiguration<HouseAmenity>
{
    public void Configure(EntityTypeBuilder<HouseAmenity> builder)
    {
        builder
            .HasKey(ha => new { ha.HouseId, ha.AmenityId });

        builder
            .HasOne(ha => ha.House)
            .WithMany(h => h.HouseAmenities)
            .HasForeignKey(ha => ha.HouseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(ha => ha.Amenity)
            .WithMany(a => a.HouseAmenities)
            .HasForeignKey(ha => ha.AmenityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ha => ha.AmenityId);
    }
}
