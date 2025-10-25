using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using ELKOOD.ToDo.Application.Interfaces;
using ELKOOD.ToDo.Application.Services;
using ELKOOD.ToDo.Application.Validators;
using ELKOOD.ToDo.Application.Mappings;
namespace ELKOOD.ToDo.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IToDoService, ToDoService>();

            // Register AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));

            // Register FluentValidation validators
            services.AddValidatorsFromAssemblyContaining<CreateToDoItemDtoValidator>();

            return services;
        }
    }
}