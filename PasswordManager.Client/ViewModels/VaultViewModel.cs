using System.ComponentModel.DataAnnotations;
using PasswordManager.Client.Services;

namespace PasswordManager.Client.ViewModels;

public class VaultViewModel
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string EncryptedKey { get; set; }
    public string Title { get; set; }
    public string Websites { get; set; }
    public bool IsSelected { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }

    public int UserId { get; set; } 
    public User User { get; set; }

    public List<GroupVault> GroupVaults { get; set; } = new List<GroupVault>();
}

public class VaultSummaries
{
    public int Id { get; set; }
    public string Title { get; set; }
}