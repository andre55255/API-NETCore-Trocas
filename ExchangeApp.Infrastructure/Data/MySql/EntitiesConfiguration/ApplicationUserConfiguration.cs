using ExchangeApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeApp.Infrastructure.Data.MySql.EntitiesConfiguration
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(x => x.Firstname).HasMaxLength(255).IsRequired(true);
            builder.Property(x => x.Lastname).HasMaxLength(255).IsRequired(true);
            builder.Property(x => x.UserName).HasMaxLength(255).IsRequired(true);
            builder.Property(x => x.NormalizedUserName).HasMaxLength(255).IsRequired(true);
            builder.Property(x => x.Email).HasMaxLength(255).IsRequired(true);
            builder.Property(x => x.NormalizedEmail).HasMaxLength(255).IsRequired(true);
            builder.Property(x => x.Whatsapp).HasMaxLength(255).IsRequired(false);
            builder.Property(x => x.Instagram).HasMaxLength(255).IsRequired(false);
        }
    }
}
