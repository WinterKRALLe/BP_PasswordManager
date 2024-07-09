using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace PasswordManager.API.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    // [HttpGet("{userId:int}")]
    // public async Task<IActionResult<UserDto>> GetUserById(int userId)
    // {
    //     var user = await userService.GetUserByIdAsync(userId);
    //     if (user == null)
    //         return NotFound();
    //
    //     var currentUserId = GetCurrentUserId();
    //
    //     return Ok(user);
    // }
    
    
}