namespace PasswordManager.Application.Interfaces;

public interface IKeyService
{
    string GenerateMasterKey();
    Task<string> EncryptKey(string userKey);
    Task<string>  DecryptKey(string encryptedUserKey);
}