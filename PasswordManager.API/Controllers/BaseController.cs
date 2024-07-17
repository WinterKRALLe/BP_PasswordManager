using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace PasswordManager.API.Controllers;

public class BaseController : ControllerBase
{
    [NonAction]
    protected int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("User ID not found in claims.");
        }

        return userId;
    }
}