using PasswordManager.Client.Services;

namespace PasswordManager.Client.ViewModels;

public class GroupViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public List<GroupUserRole> Members { get; set; } = new List<GroupUserRole>();
    public List<GroupVault> GroupVaults { get; set; } = new List<GroupVault>();
}

public enum GroupRoles
{
    Owner,
    Member
}


public class GroupUserRole
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }
    public GroupRoles Role { get; set; }

    public int GroupId { get; set; }
    public GroupViewModel Group { get; set; }
}

public class GroupVault
{
    public int Id { get; set; }
    
    public int VaultId { get; set; }
    public VaultViewModel Vault { get; set; }

    public int GroupId { get; set; }
    public GroupViewModel Group { get; set; }
}