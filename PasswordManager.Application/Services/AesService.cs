using System.Security.Cryptography;
using PasswordManager.Application.Interfaces;

namespace PasswordManager.Application.Services;

public class AesService : IAesService
{
    private const int IvSizeInBytes = 16; // Fixed IV size for AES encryption

    public async Task<string> EncryptAsync(string plainText, string key)
    {
        var keyBytes = Convert.FromBase64String(key);
        using (var aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.GenerateIV();

            MemoryStream msEncrypt;
            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            using (msEncrypt = new MemoryStream())
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                await swEncrypt.WriteAsync(plainText);
            }

            var iv = aes.IV;
            var encryptedBytes = msEncrypt.ToArray();

            var result = new byte[IvSizeInBytes + encryptedBytes.Length];
            Buffer.BlockCopy(iv, 0, result, 0, IvSizeInBytes);
            Buffer.BlockCopy(encryptedBytes, 0, result, IvSizeInBytes, encryptedBytes.Length);

            return Convert.ToBase64String(result);
        }
    }

    public async Task<string> DecryptAsync(string cipherText, string key)
    {
        var keyBytes = Convert.FromBase64String(key);
        var fullCipher = Convert.FromBase64String(cipherText);

        using (var aes = Aes.Create())
        {
            if (aes.LegalKeySizes.Any(s => s.MaxSize >= keyBytes.Length * 8 && s.MinSize <= keyBytes.Length * 8))
            {
                aes.Key = keyBytes;

                var iv = new byte[IvSizeInBytes];
                var cipher = new byte[fullCipher.Length - IvSizeInBytes];

                Buffer.BlockCopy(fullCipher, 0, iv, 0, IvSizeInBytes);
                Buffer.BlockCopy(fullCipher, IvSizeInBytes, cipher, 0, cipher.Length);

                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var msDecrypt = new MemoryStream(cipher))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    return await srDecrypt.ReadToEndAsync();
                }
            }
            else
            {
                throw new ArgumentException(
                    $"The provided key is not a valid size for the AES algorithm. Supported key sizes are: {string.Join(", ", aes.LegalKeySizes.Select(s => $"{s.MinSize / 8}-{s.MaxSize / 8} bits"))}.");
            }
        }
    }
}