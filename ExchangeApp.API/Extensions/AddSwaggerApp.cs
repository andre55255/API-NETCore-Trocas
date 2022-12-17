using Microsoft.OpenApi.Models;
using System.Reflection;

namespace ExchangeApp.API.Extensions
{
    public static class AddSwaggerApp
    {
        public static IServiceCollection AddSwaggerGenerationApp(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ExchangeApp",
                    Version = "v1",
                    Description = "API para aplicativo de trocas"
                });

                // Habilitando Swagger para ler comentários de documentação de controller
                //string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //if (!File.Exists(xmlPath))
                //    File.Create(xmlPath);

                //opt.IncludeXmlComments(xmlPath);

                // Habilitando Swagger para autenticação Jwt Bearer
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = @"JWT Authorization. Bearer token.<br/>
                    <br/>Entre com 'Bearer'[space] e depois digite o valor do token obtido no login.
                    <br/>Exemplo: \""'Bearer 12345abcdef\"""
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
            });

            return services;
        }
    }
}
