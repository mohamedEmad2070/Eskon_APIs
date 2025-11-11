
namespace Eskon_APIs.Persistance.EntitiesConfigurations;

public class MediaItemConfiguration : IEntityTypeConfiguration<MediaItem>
{
    public void Configure(EntityTypeBuilder<MediaItem> builder)
    {
        builder.HasKey(mi => mi.MediaId);

        builder.Property(mi => mi.URL)
            .IsRequired()
            .HasMaxLength(2048);

        builder
            .HasOne(mi => mi.House)
            .WithMany(h => h.MediaItems)
            .HasForeignKey(mi => mi.HouseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(m => new { m.HouseId, m.SortOrder });
        builder.HasIndex(m => m.HouseId);
    }
}
