using Eskon_APIs.Abstraction.Consts;
using Eskon_APIs.Entities;

namespace Eskon_APIs.Persistance.EntitiesConfigurations;
public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.OwnsMany(x=> x.RefreshTokens)
            .ToTable("RefreshTokens")
            .WithOwner().HasForeignKey("UserId");
        builder.Property(x => x.FirstName).HasMaxLength(100);
        builder.Property(x => x.LastName).HasMaxLength(100);

        // Seed Default Admin User
        builder.HasData(new ApplicationUser
        {
            Id = DefaultUsers.AdminId,
            FirstName = "Eskon",
            LastName = "Admin",
            UserName = DefaultUsers.AdminEmail,
            NormalizedUserName = DefaultUsers.AdminEmail.ToUpper(),
            Email = DefaultUsers.AdminEmail,
            NormalizedEmail = DefaultUsers.AdminEmail.ToUpper(),
            SecurityStamp = DefaultUsers.AdminSecurityStamp,
            ConcurrencyStamp = DefaultUsers.AdminConcurrencyStamp,
            EmailConfirmed = true,
            PasswordHash = "AQAAAAIAAYagAAAAEIW3NlZn9JYsa1r4A98wfj4CNoSIfdtdMYb7T3JlYn3ZZenM6GeinhFZqJVYCcTQ7A=="
        });
    }
}