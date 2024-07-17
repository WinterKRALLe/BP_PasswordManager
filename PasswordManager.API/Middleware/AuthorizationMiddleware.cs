using System.Security.Claims;
using System.Text.Json;
using PasswordManager.Application.Interfaces;

namespace PasswordManager.API.Middleware;

public class AuthorizationMiddleware(
    RequestDelegate next,
    IIdentityService identityService,
    IUserRepository userRepository,
    bool allowAnonymous = false)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        {
            if (allowAnonymous)
            {
                await next(context);
                return;
            }

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Missing Authorization header.");
            return;
        }

        var tokenJson = authorizationHeader.ToString().Split(' ')[1];

        // Parse the token JSON
        var tokenData = JsonSerializer.Deserialize<Dictionary<string, string>>(tokenJson);
        if (tokenData == null || !tokenData.TryGetValue("accessToken", out var token))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Invalid token format.");
            return;
        }

        // Validate the token and create a ClaimsPrincipal
        var claimsPrincipal = await identityService.ValidateTokenAsync(token);
        if (claimsPrincipal == null)
        {
            Console.WriteLine("Token validation failed.");
            if (allowAnonymous)
            {
                await next(context);
                return;
            }

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Invalid token.");
            return;
        }

        // Set the ClaimsPrincipal on the HttpContext
        context.User = claimsPrincipal;

        // Access the claims from context.User.Identity
        var claimsIdentity = context.User.Identity as ClaimsIdentity;
        var userName = claimsIdentity?.FindFirst(ClaimTypes.Name)?.Value;

        if (userName == null || !await userRepository.UserExistsAsync(userName))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: User not found.");
            return;
        }

        await next(context);
    }
}