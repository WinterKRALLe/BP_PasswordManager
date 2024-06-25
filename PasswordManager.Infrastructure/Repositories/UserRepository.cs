using Microsoft.EntityFrameworkCore;
using PasswordManager.Domain.Entities;
using PasswordManager.Domain.Repositories;
using PasswordManager.Infrastructure.Database.Context;

namespace PasswordManager.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext dbContextFactory) : IUserRepository
{
    protected readonly ApplicationDbContext Context = dbContextFactory;

    public async Task<RefreshToken> AddAsync(RefreshToken token)
    {
        await Context.RefreshTokens.AddAsync(token);
        await Context.SaveChangesAsync();
        return token; 
    }
    
    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await Context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
    }

    public async Task DeleteByUserIdAsync(int userId)
    {
        var tokensToDelete = await Context.RefreshTokens.Where(rt => rt.UserId == userId).ToListAsync();
        Context.RefreshTokens.RemoveRange(tokensToDelete);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteAsync(RefreshToken refreshToken)
    {
        Context.RefreshTokens.Remove(refreshToken);
        await Context.SaveChangesAsync();
    }
    
    public async Task AddUser(User user)
    {
        Context.Users.Add(user);
        await Context.SaveChangesAsync();
    }
    
    public async Task<bool> UserExistsAsync(string username)
    {
        return await Context.Users.AnyAsync(u => u.UserName == username);
    }
    
    public async Task<User?> GetByIdAsync(int id)
    {
        return await Context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByUsernameAsync(string email)
    {
        return await Context.Users
            .FirstOrDefaultAsync(u => u.UserName == email);
    }
}