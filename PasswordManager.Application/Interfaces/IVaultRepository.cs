using PasswordManager.Application.DTOs;
using PasswordManager.Domain.Entities;

namespace PasswordManager.Application.Interfaces;

public interface IVaultRepository
{
    Task<List<Vault>> GetVaultsAsync(int userId);
    Task<List<Vault>> GetDeletedVaultsAsync(int userId);
    Task<List<Vault>> GetPersonalVaultsAsync(int userId);
    Task<bool> CreateLoginAsync(AddVaultDto model, int userId);
    Task<bool> MoveToTrashAsync(int vaultId);
}