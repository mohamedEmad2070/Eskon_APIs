
namespace Eskon_APIs.Persistance.EntitiesConfigurations;

public class AmenityConfiguration : IEntityTypeConfiguration<Amenity>
{
    public void Configure(EntityTypeBuilder<Amenity> builder)
    {
        builder.HasKey(a => a.AmenityId);

        builder.Property(a => a.AmenityName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Category)
            .HasMaxLength(100);

        builder.HasIndex(a => a.AmenityName)
            .IsUnique()
            .HasDatabaseName("IX_Amenity_Name");
        builder.HasIndex(a => new { a.Category, a.AmenityName }).HasDatabaseName("IX_Amenity_Category_Name");
    }
}
