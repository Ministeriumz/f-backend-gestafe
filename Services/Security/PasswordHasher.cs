using BCrypt.Net;

namespace f_backend_gestafe.Services.Security
{
    public static class PasswordHasher
    {
        public static string Hash(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha))
            {
                throw new ArgumentException("Senha inválida.", nameof(senha));
            }

            return BCrypt.Net.BCrypt.HashPassword(senha);
        }

        public static bool Verify(string senha, string hash)
        {
            if (string.IsNullOrWhiteSpace(senha) || string.IsNullOrWhiteSpace(hash))
            {
                return false;
            }

            return BCrypt.Net.BCrypt.Verify(senha, hash);
        }
    }
}
