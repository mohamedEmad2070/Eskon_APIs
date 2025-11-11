
namespace Eskon_APIs.Persistance.EntitiesConfigurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        #region Attributes Configurations
        builder.HasKey(l => l.LocationId);

        builder.Property(l => l.Country)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(l => l.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(l => l.PostalCode)
            .HasMaxLength(20);

        builder.Property(l => l.Street)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(l => l.BuildingNumber)
            .IsRequired()
            .HasMaxLength(20); 
        #endregion

        builder.HasMany(l => l.Houses)
                   .WithOne(h => h.Location)
                   .HasForeignKey(h => h.LocationId)
                   .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(l => new { l.City, l.PostalCode });
        builder.HasIndex(l => new { l.City, l.Street, l.BuildingNumber })
               .HasDatabaseName("IX_Location_Address");
    }
}
