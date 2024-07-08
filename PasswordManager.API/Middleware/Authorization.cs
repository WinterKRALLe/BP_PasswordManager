using System.Security.Claims;
using PasswordManager.Application.Interfaces;
using PasswordManager.Domain.Repositories;

namespace PasswordManager.API.Middleware;

public class Authorization(RequestDelegate next, IIdentityService identityService, IUserRepository userRepository, bool allowAnonymous = false)
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

        var token = authorizationHeader.ToString().Split(' ')[1];

        // Validate the token and create a ClaimsPrincipal
        var claimsPrincipal = await identityService.ValidateTokenAsync(token);
        if (claimsPrincipal == null)
        {
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