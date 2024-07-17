using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Application.DTOs;

public class AddGroupDto
{
    [Required] public string GroupName { get; set; } = null!;

    public List<string> ShareWith { get; set; } = null!;
}

public class GroupFormDto
{
    public List<GroupDto> Groups { get; set; } = new List<GroupDto>();
}

public class GroupDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool IsChecked { get; set; }
}

public class AddUserToGroupDto
{
    [Required] [EmailAddress] public string UserName { get; set; }
}