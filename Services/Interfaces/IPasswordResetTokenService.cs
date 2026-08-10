using f_backend_gestafe.Objects.Models;

namespace f_backend_gestafe.Services.Interfaces
{
    public record PasswordResetTokenData(int UserId, string PasswordVersion);

    public interface IPasswordResetTokenService
    {
        (string Token, DateTime ExpiresAt) Create(Usuario usuario);
        PasswordResetTokenData? Validate(string token);
        bool IsCurrent(Usuario usuario, string passwordVersion);
    }
}
