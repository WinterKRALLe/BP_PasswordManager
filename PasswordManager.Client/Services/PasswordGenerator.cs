namespace PasswordManager.Client.Services;

public static class PasswordGenerator
{
    public static string GeneratePassword(int passwordLength, bool includeSpecialCharacters, bool includeCapitalLetters,
        bool includeNumbers)
    {
        var charSet = "abcdefghijklmnopqrstuvwxyz";
        var random = new Random();
        var passwordChars = new List<char>();

        if (includeSpecialCharacters)
        {
            charSet += "!@#$%^&*()_+-=[]{}|;:,.<>?";
            passwordChars.Add("!@#$%^&*()_+-=[]{}|;:,.<>? "[random.Next("!@#$%^&*()_+-=[]{}|;:,.<>? ".Length)]);
        }

        if (includeCapitalLetters) 
        {
            charSet += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            passwordChars.Add("ABCDEFGHIJKLMNOPQRSTUVWXYZ"[random.Next("ABCDEFGHIJKLMNOPQRSTUVWXYZ".Length)]);
        }

        if (includeNumbers)
        {
            charSet += "0123456789";
            passwordChars.Add("0123456789"[random.Next("0123456789".Length)]);
        }

        for (int i = passwordChars.Count; i < passwordLength; i++)
        {
            passwordChars.Add(charSet[random.Next(charSet.Length)]);
        }

        passwordChars = passwordChars.OrderBy(_ => random.Next()).ToList();
        return new string(passwordChars.ToArray());
    }
}