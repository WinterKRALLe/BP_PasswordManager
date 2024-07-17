using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Client.ViewModels;

public class UserViewModel
{
    public int Id { get; set; }
    [EmailAddress] public string UserName { get; set; }
}