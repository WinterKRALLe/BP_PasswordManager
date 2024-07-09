using System.Security.Cryptography;
using PasswordManager.Application.Interfaces;

namespace PasswordManager.Application.Services;

public class KeyService(IAesService aesService) : IKeyService
{
    private readonly byte[] MasterKey = Convert.FromBase64String("U2FsdGVkX1/TxeFjRZ+3C8/KeJPinIGx4ZuEEAqOQ3Q=");
    
    public string GenerateMasterKey()
    {
        using (var aes = Aes.Create())
        {
            aes.KeySize = 128;
            aes.GenerateKey();
            return Convert.ToBase64String(aes.Key);
        }
    }
    
    public async Task<string> EncryptKey(string userKey)
    {
        return await aesService.EncryptAsync(userKey, Convert.ToBase64String(MasterKey));
    }

    public async Task<string>  DecryptKey(string encryptedUserKey)
    {
        return await aesService.DecryptAsync(encryptedUserKey, Convert.ToBase64String(MasterKey));
    }
}