
namespace Eskon_APIs.Persistance.EntitiesConfigurations;

public class SavedListConfiguration : IEntityTypeConfiguration<SavedList>
{
    public void Configure(EntityTypeBuilder<SavedList> builder)
    {
        builder.HasKey(s => new { s.UserId, s.HouseId });

        builder
            .HasOne(s => s.User)
            .WithMany(u => u.SavedLists)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(s => s.House)
            .WithMany(h => h.SavedBy)
            .HasForeignKey(s => s.HouseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => s.UserId);
    }
}
