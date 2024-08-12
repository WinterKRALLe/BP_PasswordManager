namespace PasswordManager.Domain.Entities;

public class Group
{
    public int Id { get; set; }
    public string Title { get; set; }

    public List<GroupUserRole>? Members { get; set; } = new List<GroupUserRole>();
    public List<GroupVault>? GroupVaults { get; set; } = new List<GroupVault>();
}