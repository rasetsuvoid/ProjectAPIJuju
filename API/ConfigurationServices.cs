using Juju.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.Configuration;

namespace API
{
    public static class ConfigurationServices
    {
        public static void AddConfigurationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Agregar cadena de conexion al contexto
            services.AddDbContext<JujuTestContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Development")));

             //======== CONFIGURACIÓN DE SWAGGER =========
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "TestAPI", Version = "v1" });
            });
        }
    }
}
