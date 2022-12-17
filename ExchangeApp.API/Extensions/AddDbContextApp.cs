using ExchangeApp.Infrastructure.Data.Pg.Context;
using Microsoft.EntityFrameworkCore;

namespace ExchangeApp.API.Extensions
{
    public static class AddDbContextApp
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            string pgConn = configuration.GetConnectionString("Pg");

            services.AddDbContext<PgDbContext>(opt =>
            {
                opt.UseNpgsql(pgConn,
                    builder => builder.EnableRetryOnFailure());
            }, ServiceLifetime.Scoped, ServiceLifetime.Scoped);

            return services;
        }
    }
}
