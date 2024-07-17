using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PasswordManager.Client.ViewModels;

public class AddLoginViewModel
{
    [JsonPropertyName("Title")] [Required] public string Title { get; set; } = null!;

    [JsonPropertyName("UserName")]
    [Required]
    public string UserName { get; set; } = null!;

    [JsonPropertyName("Password")]
    [Required]
    public string Password { get; set; } = null!;

    [JsonPropertyName("Website")] public string Websites { get; set; } = null!;
}