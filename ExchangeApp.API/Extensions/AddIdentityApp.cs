using ExchangeApp.Core.Entities;
using ExchangeApp.Helpers;
using ExchangeApp.Infrastructure.Data.Pg.Context;
using Microsoft.AspNetCore.Identity;

namespace ExchangeApp.API.Extensions
{
    public static class AddIdentityApp
    {
        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, RoleUser>(opt =>
                       opt.SignIn.RequireConfirmedEmail = true
                   )
                   .AddEntityFrameworkStores<PgDbContext>()
                   .AddDefaultTokenProviders()
                   .AddTokenProvider(Tokens.LoginProviderRefreshTokenApp,
                                     typeof(DataProtectorTokenProvider<ApplicationUser>));

            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequiredLength = 6;
                opt.Password.RequiredUniqueChars = 3;
            });

            return services;
        }
    }
}
