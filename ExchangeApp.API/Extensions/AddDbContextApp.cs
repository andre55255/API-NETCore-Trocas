using ExchangeApp.Infrastructure.Data.MySql.Context;
using Microsoft.EntityFrameworkCore;

namespace ExchangeApp.API.Extensions
{
    public static class AddDbContextApp
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            string mySqlConn = configuration.GetConnectionString("MySql");

            services.AddDbContext<MySqlDbContext>(opt =>
            {
                opt.UseMySql(mySqlConn, new MySqlServerVersion(new Version(8, 0)));
            }, ServiceLifetime.Scoped, ServiceLifetime.Scoped);

            return services;
        }
    }
}
