namespace PasswordManager.Domain.Entities;

public class GroupVault
{
    public int Id { get; set; }
    
    public int VaultId { get; set; }
    public Vault Vault { get; set; }

    public int GroupId { get; set; }
    public Group Group { get; set; }
}