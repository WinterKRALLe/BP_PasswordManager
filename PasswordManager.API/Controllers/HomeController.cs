using Microsoft.AspNetCore.Mvc;
using PasswordManager.Application.DTOs;

namespace PasswordManager.API.Controllers;

[Route("[controller]")]
[ApiController]
public class HomeController: ControllerBase
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