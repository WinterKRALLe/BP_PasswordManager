using Microsoft.EntityFrameworkCore;
using PasswordManager.Domain.Entities;

namespace PasswordManager.Infrastructure.Database.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}
