using Microsoft.AspNetCore.Mvc;
using PasswordManager.Application.DTOs;
using PasswordManager.Application.Interfaces;
using PasswordManager.Domain.Repositories;

namespace PasswordManager.API.Controllers;

[Route("[controller]")]
[ApiController]
public class IdentityController(IUserRepository userRepository, IIdentityService identityService) : ControllerBase
{
    [HttpPost("register")] 
    public async Task<IActionResult> Register([FromBody] IdentityDto newUser)
    {
        var result = await identityService.RegisterAsync(newUser);

        if (result)
        {
            return Ok("User registered successfully."); 
        }
        return BadRequest("Username or email is already taken.");
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] IdentityDto login)
    {
        var token = await identityService.LoginUserAsync(login);

        if (token is not null)
        {
            return Ok(token);
        }

        return Unauthorized("Invalid username or password."); 
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
    {
        var storedRefreshToken = await userRepository.GetByTokenAsync(refreshTokenDto.Token);
        if (storedRefreshToken == null || storedRefreshToken.Expires < DateTime.UtcNow)
        {
            return Unauthorized("Invalid refresh token.");
        }
        
        await userRepository.DeleteAsync(storedRefreshToken);
        Console.WriteLine($"Token: {storedRefreshToken.Token} Deleted successfully.");

        var userId = storedRefreshToken.UserId; 
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null) 
        {
            return Unauthorized("User associated with refresh token not found.");
        }

        var newTokens = identityService.GenerateJwtToken(userId, user.UserName);
        
        return Ok(newTokens);
    }
}