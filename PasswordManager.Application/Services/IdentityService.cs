using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PasswordManager.Application.DTOs;
using PasswordManager.Application.Interfaces;
using PasswordManager.Domain.Entities;
using PasswordManager.Domain.Repositories;

namespace PasswordManager.Application.Services;

public class IdentityService(IConfiguration configuration, IUserRepository userRepository)
    : IIdentityService
{
    public async Task<bool> RegisterAsync(IdentityDto newUser)
    {
        if (await userRepository.UserExistsAsync(newUser.UserName))
        {
            return false;
        }

        var user = new User
        {
            UserName = newUser.UserName,
            PasswordHash = HashPassword(newUser.Password, newUser.UserName)
        };

        await userRepository.AddUser(user);
        return true;
    }

    public async Task<JwtTokenDto?> LoginUserAsync(IdentityDto login)
    {
        var user = await userRepository.GetByUsernameAsync(login.UserName);

        if (user == null || !VerifyPassword(login.Password, login.UserName, user.PasswordHash))
        {
            return null;
        }

        return await GenerateJwtToken(user.Id, user.UserName);
    }

    public async Task<JwtTokenDto> GenerateJwtToken(int userId, string userName)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration.GetSection("Jwt:Secret").Value!);
        var accessTokenExpiration = int.Parse(configuration.GetSection("Jwt:AccessTokenExpirationInDays").Value!);
        var refreshTokenExpiration = int.Parse(configuration.GetSection("Jwt:RefreshTokenExpirationInDays").Value!);

        // Access Token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.NameIdentifier, userId.ToString()),
                new(ClaimTypes.Name, userName)
            }),
            Expires = DateTime.UtcNow.AddDays(accessTokenExpiration),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var accessToken = tokenHandler.CreateToken(tokenDescriptor);
        var refreshToken = GenerateRefreshToken();
        var refreshExpirationDate = DateTime.UtcNow.AddDays(refreshTokenExpiration);

        // Store the refresh token in the database
        await userRepository.AddAsync(new RefreshToken
            { UserId = userId, Token = refreshToken, Expires = refreshExpirationDate });

        return new JwtTokenDto
        {
            AccessToken = tokenHandler.WriteToken(accessToken),
            AccessTokenExpiration = tokenDescriptor.Expires.Value,
            RefreshToken = refreshToken,
            RefreshTokenExpiration = refreshExpirationDate
        };
    }

    private string HashPassword(string password, string email)
    {
        string salt = $"{email}%salting*";
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: Encoding.ASCII.GetBytes(salt),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
    }

    private bool VerifyPassword(string password, string email, string hashedPassword)
    {
        return HashPassword(password, email) == hashedPassword;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public Task<ClaimsPrincipal?> ValidateTokenAsync(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration.GetSection("Jwt:Secret").Value!);
            var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = true
            }, out _);

            return Task.FromResult(claimsPrincipal)!;
        }
        catch
        {
            return Task.FromResult<ClaimsPrincipal>(null!)!;
        }
    }
}