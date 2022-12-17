using ExchangeApp.Core.Entities;
using ExchangeApp.Infrastructure.Data.Pg.EntitiesConfiguration;
using ExchangeApp.Infrastructure.Data.Pg.Seed;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExchangeApp.Infrastructure.Data.Pg.Context
{
    public class PgDbContext : IdentityDbContext<ApplicationUser, RoleUser, string>
    {
        public PgDbContext(DbContextOptions<PgDbContext> opt) : base(opt)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.UseIdentityAlwaysColumns();

            // Seed
            builder.ApplyConfiguration(new AddUserDefault());
            builder.ApplyConfiguration(new AddRolesDefault());
            builder.ApplyConfiguration(new AddRelationUserRoleDefault());

            // Configs
            builder.ApplyConfiguration(new ApplicationUserConfiguration());
        }
    }
}
