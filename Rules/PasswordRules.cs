namespace Asilo.Core.Rules;

public static class PasswordRules
{
    public static bool IsValid(string password)
    {
        if (string.IsNullOrEmpty(password)) return false;
        if (password.Length < 8) return false;
        if (!password.Any(char.IsUpper)) return false;
        if (!password.Any(char.IsLower)) return false;
        if (!password.Any(char.IsDigit)) return false;
        // agregar símbolos si se desea
        return true;
    }
}