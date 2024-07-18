namespace PasswordManager.Application.DTOs;

public class AddVaultDto
{
    public string Title { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }

    public string? Websites { get; set; }
}

public class VaultSummaryDto
{
    public int Id { get; set; }
    public string Title { get; set; }
}