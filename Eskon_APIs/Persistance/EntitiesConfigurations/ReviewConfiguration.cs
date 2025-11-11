
namespace Eskon_APIs.Persistance.EntitiesConfigurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.ReviewId);

        builder.Property(r => r.Stars)
            .IsRequired();

        builder.Property(r => r.Comment)
            .HasMaxLength(1000);

        #region Relation Configuration
        builder
            .HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(r => r.House)
            .WithMany(h => h.Reviews)
            .HasForeignKey(r => r.HouseId)
            .OnDelete(DeleteBehavior.Cascade);
        #endregion

        builder.HasIndex(r => new { r.UserId, r.HouseId }).IsUnique();

        builder.HasIndex(r => r.HouseId);
        builder.HasIndex(r => r.UserId);
    }
}
