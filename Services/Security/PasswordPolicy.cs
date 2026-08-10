using System.Text.RegularExpressions;

namespace f_backend_gestafe.Services.Security
{
    public static class PasswordPolicy
    {
        public const string Pattern = @"^(?=.{8,255}$)(?=.*[A-Z])(?=.*\d)(?=.*[^\p{L}\p{N}\s]).*$";
        public const string ErrorMessage = "A senha deve ter no mínimo 8 caracteres, uma letra maiúscula, um número e um caractere especial.";

        public static bool IsValid(string password)
        {
            return !string.IsNullOrWhiteSpace(password) && Regex.IsMatch(password, Pattern);
        }
    }
}
