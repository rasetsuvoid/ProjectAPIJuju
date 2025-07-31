using Juju.Application.Contracts;
using Juju.Application.Contracts.Services;
using Juju.Application.Services;
using Juju.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Juju.Infrastructure
{
    public static class ConfigurationServices
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IPostRepository, PostRepository>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        }
    }
}
