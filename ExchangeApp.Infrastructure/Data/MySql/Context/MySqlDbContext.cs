using ExchangeApp.Core.Entities;
using ExchangeApp.Infrastructure.Data.MySql.EntitiesConfiguration;
using ExchangeApp.Infrastructure.Data.MySql.Seed;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExchangeApp.Infrastructure.Data.MySql.Context
{
    public class MySqlDbContext : IdentityDbContext<ApplicationUser, RoleUser, string>
    {
        public MySqlDbContext(DbContextOptions<MySqlDbContext> opt) : base(opt)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed
            builder.ApplyConfiguration(new AddUserDefault());
            builder.ApplyConfiguration(new AddRolesDefault());
            builder.ApplyConfiguration(new AddRelationUserRoleDefault());

            // Configs
            builder.ApplyConfiguration(new ApplicationUserConfiguration());
        }
    }
}
