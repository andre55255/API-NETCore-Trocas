using ExchangeApp.Core.ServicesInterface;
using ExchangeApp.Infrastructure.ServicesImpl;

namespace ExchangeApp.API.Extensions
{
    public static class AddServicesApp
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();

            return services;
        }
    }
}
