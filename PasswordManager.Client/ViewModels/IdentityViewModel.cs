using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Client.ViewModels;

public class LoginViewModel
{
    [Required] [EmailAddress] public string UserName { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
}

public class RegisterViewModel
{
    [Required] [EmailAddress] public string Email { get; set; } = null!;

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
        MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Passwords don't match!")]
    public string RepeatedPassword { get; set; } = null!;
}