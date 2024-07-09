using Microsoft.AspNetCore.Mvc.Filters;
using PasswordManager.Application.Interfaces;

namespace PasswordManager.API.Middleware;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AuthorizeMiddlewareAttribute(bool allowAnonymous = false) : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var httpContext = context.HttpContext;
        var authService = httpContext.RequestServices.GetRequiredService<IIdentityService>();
        var userRepository = httpContext.RequestServices.GetRequiredService<IUserRepository>();

        var middleware = new AuthorizationMiddleware(async _ => await next(), authService, userRepository, allowAnonymous);
        await middleware.InvokeAsync(httpContext);
    }
}