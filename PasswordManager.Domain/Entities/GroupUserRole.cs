using PasswordManager.Domain.Enums;

namespace PasswordManager.Domain.Entities;

public class GroupUserRole
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }
    public GroupRoles Role { get; set; }

    public int GroupId { get; set; }
    public Group Group { get; set; }
}