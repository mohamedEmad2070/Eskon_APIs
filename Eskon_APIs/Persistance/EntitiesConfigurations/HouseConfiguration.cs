
namespace Eskon_APIs.Persistance.EntitiesConfigurations;

public class HouseConfiguration : IEntityTypeConfiguration<House>
{
    public void Configure(EntityTypeBuilder<House> builder)
    {
        #region Attribute Configuration
        builder.HasKey(h => h.HouseId);

        builder.Property(h => h.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(h => h.Description)
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(h => h.PricePerMonth)
            .HasPrecision(18, 3);
        #endregion

        #region Relation Configuration
        builder
            .HasOne(h => h.Owner)
            .WithMany(u => u.OwnedHouses)
            .HasForeignKey(h => h.OwnerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(h => h.Location)
            .WithMany(l => l.Houses)
            .HasForeignKey(h => h.LocationId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict); 
        #endregion

        builder.HasIndex(h => h.LocationId);
        builder.HasIndex(h => h.PricePerMonth);
        builder.HasIndex(h => new { h.LocationId, h.PricePerMonth });
    }
}
