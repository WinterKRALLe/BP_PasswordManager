using Microsoft.EntityFrameworkCore;
using PasswordManager.Domain.Entities;

namespace PasswordManager.Infrastructure.Database.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Vault> Vaults { get; set; }
    public DbSet<GroupUserRole> GroupUserRoles { get; set; }
    public DbSet<GroupVault> GroupVaults { get; set; }
}