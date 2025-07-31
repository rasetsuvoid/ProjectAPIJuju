using FluentValidation;
using Juju.Application.Contracts.Services;
using Juju.Application.Dtos;
using Juju.Application.Mapping;
using Juju.Application.Services;
using Juju.Application.Validations;
using Juju.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace Juju.Application
{
    public static class ConfigurationServices
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICustomerServices, CustomerServices>();
            services.AddScoped<IPostServices, PostServices>();

            services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<Customer, CustomerDto>();
                cfg.CreateMap<CustomerDto, Customer>();
                cfg.CreateMap<Post, PostDto>();
                cfg.CreateMap<PostDto, Post>();
                cfg.CreateMap<Customer, CustomerRequest>();
                cfg.CreateMap<CustomerRequest, Customer>();
                cfg.CreateMap<Post, PostRequest>();
                cfg.CreateMap<PostRequest, Post>();
            });

            services.AddScoped<IValidator<CustomerRequest>, CustomerValidator>();
            services.AddScoped<IValidator<CustomerDto>, UpdateCustomerValidator>();

            services.AddScoped<IValidator<PostRequest>, PostRequestValidator>();
            services.AddScoped<IValidator<PostUpdate>, UpdatePostValidator>();

        }
    }
}
