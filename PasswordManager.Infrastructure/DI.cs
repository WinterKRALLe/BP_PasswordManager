using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Application.Interfaces;
using PasswordManager.Infrastructure.Repositories;

namespace PasswordManager.Infrastructure;

public static class DI
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IVaultRepository, VaultRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        
        return services;
    }
}