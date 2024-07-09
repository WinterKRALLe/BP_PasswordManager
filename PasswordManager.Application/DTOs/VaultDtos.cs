using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Application.DTOs;

public class AddVaultDto
{
    [Required] public string Title { get; set; } = null!;

    [Required] public string UserName { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    public string Websites { get; set; } = null!;
}