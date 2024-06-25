using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Domain.Repositories;
using PasswordManager.Infrastructure.Repositories;

namespace PasswordManager.Infrastructure;

public static class DI
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        
        return services;
    }
}