using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace PasswordManager.Client.Services;

public class User(AuthenticationStateProvider authStateProvider)
{
    public async Task<ClaimsPrincipal> GetUserAsync()
    {
        var authState = await authStateProvider.GetAuthenticationStateAsync();
        return authState.User;
    }

    public async Task<string> GetUsernameAsync()
    {
        var user = await GetUserAsync(); 
        return user.FindFirst(ClaimTypes.Name)?.Value;
    }
}