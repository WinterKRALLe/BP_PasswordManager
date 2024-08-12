using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PasswordManager.Client.ViewModels;

public class AddGroupViewModel
{
    [JsonPropertyName("GroupName")] public string GroupName { get; set; }

    [JsonPropertyName("ShareWith")] public List<string>? ShareWith { get; set; }
}

public class AddUserToGroup
{
    [JsonPropertyName("Email")]
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}

public class GroupViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsChecked { get; set; }
    public List<GroupUserRole>? Members { get; set; }
    public List<GroupVault>? GroupVaults { get; set; }
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
    public UserViewModel User { get; set; }
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
    [JsonIgnore] public GroupViewModel Group { get; set; }
}