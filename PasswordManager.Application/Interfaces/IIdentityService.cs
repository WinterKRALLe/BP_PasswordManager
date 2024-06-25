using System.Security.Claims;
using PasswordManager.Application.DTOs;

namespace PasswordManager.Application.Interfaces;

public interface IIdentityService
{
    Task<bool> RegisterAsync(IdentityDto newUser);
    Task<JwtTokenDto?> LoginUserAsync(IdentityDto login);
    Task<ClaimsPrincipal?> ValidateTokenAsync(string token);
    Task<JwtTokenDto> GenerateJwtToken(int userId, string userName);
}