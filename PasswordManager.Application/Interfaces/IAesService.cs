namespace PasswordManager.Application.Interfaces;

public interface IAesService
{
    Task<string> EncryptAsync(string plainText, string key);
    Task<string> DecryptAsync(string cipherText, string key);
}