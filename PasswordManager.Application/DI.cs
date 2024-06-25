using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Application.Interfaces;
using PasswordManager.Application.Services;

namespace PasswordManager.Application;

public static class DI
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // services.AddScoped<IPasswordGeneratorService, PasswordGeneratorService>();
        services.AddScoped<IIdentityService, IdentityService>();
        // services.AddScoped<IAesEncryption, AesEncryption>();
        // services.AddScoped<IKeyManager, KeyManager>();
        
        return services;
    }
}