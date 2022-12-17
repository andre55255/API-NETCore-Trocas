namespace ExchangeApp.API.Extensions
{
    public static class AddCorsApp
    {
        public static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy("ClientPermission", policy =>
                {
                    policy.AllowAnyHeader()
                          .AllowAnyMethod()
                          .WithOrigins(
                            new string[]
                            {
                                configuration["SSLURL"] + configuration["HostedURL"],
                                configuration["SSLURL"] + "www." + configuration["HostedURL"],
                            })
                          .AllowCredentials();
                });
            });

            return services;
        }
    }
}
