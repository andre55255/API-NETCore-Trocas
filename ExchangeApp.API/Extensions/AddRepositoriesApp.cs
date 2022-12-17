using ExchangeApp.Core.RepositoriesInterface;
using ExchangeApp.Infrastructure.RepositoriesImpl;

namespace ExchangeApp.API.Extensions
{
    public static class AddRepositoriesApp
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
