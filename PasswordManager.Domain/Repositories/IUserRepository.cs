using PasswordManager.Domain.Entities;

namespace PasswordManager.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByUsernameAsync(string email);
    Task<bool> UserExistsAsync(string username);
    Task AddUser(User user);
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task DeleteByUserIdAsync(int userId);
    Task DeleteAsync(RefreshToken refreshToken);
    Task<RefreshToken> AddAsync(RefreshToken token);
}