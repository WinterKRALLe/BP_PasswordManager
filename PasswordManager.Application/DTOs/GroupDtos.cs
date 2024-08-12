using System.ComponentModel.DataAnnotations;
using PasswordManager.Domain.Entities;

namespace PasswordManager.Application.DTOs;

public class AddGroupDto
{
    [Required] public string GroupName { get; set; }

    public List<string>? ShareWith { get; set; } = null;
}

public class ShareVaultDto
{
    public Vault Vault { get; set; }
    public List<GroupDto> Groups { get; set; }
}

public class GroupFormDto
{
    public List<GroupDto> Groups { get; set; } = new List<GroupDto>();
}

public class ShareWithGroupDto
{
    public List<int> GroupsId { get; set; }
    public int VaultId { get; set; }
}

public class GroupDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsChecked { get; set; }
}

public class AddUserToGroupDto
{
    [Required] [EmailAddress] public string Email { get; set; }
}