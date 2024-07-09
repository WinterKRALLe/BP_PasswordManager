using Microsoft.EntityFrameworkCore;
using PasswordManager.Application.DTOs;
using PasswordManager.Application.Interfaces;
using PasswordManager.Domain.Entities;
using PasswordManager.Infrastructure.Database.Context;

namespace PasswordManager.Infrastructure.Repositories;

public class VaultRepository(ApplicationDbContext dbContextFactory, IKeyService keyService, IAesService aesService) : IVaultRepository
{
    protected readonly ApplicationDbContext Context = dbContextFactory;
    
    public async Task<List<Vault>> GetVaultsAsync(int userId)
    {
        return await Context.Vaults.Where(c => c.UserId == userId && c.IsDeleted == false).ToListAsync();
    }
    
    public async Task<List<Vault>> GetDeletedVaultsAsync(int userId)
    {
        return await Context.Vaults.Where(c => c.UserId == userId && c.IsDeleted).ToListAsync();
    }
    
    public async Task<List<Vault>> GetPersonalVaultsAsync(int userId)
    {
        return await Context.Vaults.Where(c => c.UserId == userId).ToListAsync();
    }

    public async Task<bool> CreateLoginAsync(AddVaultDto model, int userId)
    {
        var key = keyService.GenerateMasterKey();
        var encryptedKey = await keyService.EncryptKey(key);
        var encryptedPassword = await aesService.EncryptAsync(model.Password, key);

        var vault = new Vault
        {
            Title = model.Title,
            Username = model.UserName,
            Password = encryptedPassword,
            EncryptedKey = encryptedKey,
            UserId = userId,
            CreatedAt = DateTime.Now
        };

        Context.Vaults.Add(vault);
        return await Context.SaveChangesAsync() > 0;
    }

    public async Task<bool> MoveToTrashAsync(int vaultId)
    {
        var vault = await Context.Vaults.FindAsync(vaultId);
        if (vault == null) return false;

        vault.IsDeleted = true;
        Context.Entry(vault).State = EntityState.Modified;
        return await Context.SaveChangesAsync() > 0;
    }
}