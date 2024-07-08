namespace PasswordManager.Domain.Entities;

public class Vault
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string EncryptedKey { get; set; }
    public string Title { get; set; }
    public bool IsSelected { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public List<GroupVault> GroupVaults { get; set; } = new List<GroupVault>();
}