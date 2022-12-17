using ExchangeApp.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeApp.Infrastructure.Data.Pg.Seed
{
    public class AddUserDefault : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            ApplicationUser admin = new ApplicationUser
            {
                Id = "2deb000b57bfac9d72c14d4ed967b572",
                Firstname = "Admin",
                Lastname = "Exchange",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@uorak.com",
                NormalizedEmail = "ADMIN@UORAK.COM",
                EmailConfirmed = true,
                AccessFailedCount = 0,
                Instagram = null,
                Whatsapp = "31995600166",
                PhoneNumber = "31995600166",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();
            admin.PasswordHash = hasher.HashPassword(admin, "abc123");

            builder.HasData(admin);
        }
    }

    public class AddRolesDefault : IEntityTypeConfiguration<RoleUser>
    {
        public void Configure(EntityTypeBuilder<RoleUser> builder)
        {
            builder.HasData(
                new RoleUser
                {
                    Id = "d78c03d72e72b44a131d255aec3c8a11",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new RoleUser
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "User",
                    NormalizedName = "USER"
                }
            );
        }
    }

    public class AddRelationUserRoleDefault : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(new IdentityUserRole<string>
            {
                RoleId = "d78c03d72e72b44a131d255aec3c8a11",
                UserId = "2deb000b57bfac9d72c14d4ed967b572"
            });
        }
    }
}
