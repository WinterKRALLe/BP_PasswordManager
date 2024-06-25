using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Domain.Entities;

public class User
{
    [Key] public int Id { get; set; }
    [Required] [EmailAddress] public string UserName { get; set; }
    [Required] public string PasswordHash { get; set; }
    
}