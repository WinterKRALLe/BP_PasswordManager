using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Client.ViewModels;

public class AddLoginViewModel
{
    [Required] public string Title { get; set; } = null!;

    [Required] public string UserName { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    public string Websites { get; set; } = null!;
}