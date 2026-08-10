using f_backend_gestafe.Configurations;
using f_backend_gestafe.Objects.Models;
using f_backend_gestafe.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace f_backend_gestafe.Services.Security
{
    public class PasswordResetTokenService : IPasswordResetTokenService
    {
        private const string PurposeClaim = "p";
        private const string PasswordVersionClaim = "v";
        private const string PasswordResetPurpose = "reset";

        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expiresMinutes;

        public PasswordResetTokenService(IConfiguration configuration, IOptions<PasswordResetSettings> settings)
        {
            _key = configuration["Jwt:Key"] ?? throw new InvalidOperationException("Chave JWT não configurada.");
            _issuer = configuration["Jwt:Issuer"] ?? "f-backend-gestafe";
            var accessTokenAudience = configuration["Jwt:Audience"] ?? "f-backend-gestafe-client";
            _audience = $"{accessTokenAudience}:password-reset";
            _expiresMinutes = settings.Value.ExpiresMinutes > 0 ? settings.Value.ExpiresMinutes : 30;
        }

        public (string Token, DateTime ExpiresAt) Create(Usuario usuario)
        {
            var expiresAt = DateTime.UtcNow.AddMinutes(_expiresMinutes);
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims:
                [
                    new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                    new Claim(PurposeClaim, PasswordResetPurpose),
                    new Claim(PasswordVersionClaim, CreatePasswordVersion(usuario.Senha))
                ],
                expires: expiresAt,
                signingCredentials: credentials);

            return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
        }

        public PasswordResetTokenData? Validate(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            try
            {
                var handler = new JwtSecurityTokenHandler { MapInboundClaims = false };
                var principal = handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
                    ValidAlgorithms = [SecurityAlgorithms.HmacSha256],
                    ClockSkew = TimeSpan.Zero
                }, out _);

                if (principal.FindFirstValue(PurposeClaim) != PasswordResetPurpose ||
                    !int.TryParse(principal.FindFirstValue(JwtRegisteredClaimNames.Sub), out var userId))
                {
                    return null;
                }

                var passwordVersion = principal.FindFirstValue(PasswordVersionClaim);
                return string.IsNullOrWhiteSpace(passwordVersion)
                    ? null
                    : new PasswordResetTokenData(userId, passwordVersion);
            }
            catch (SecurityTokenException)
            {
                return null;
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        public bool IsCurrent(Usuario usuario, string passwordVersion)
        {
            var currentVersion = CreatePasswordVersion(usuario.Senha);
            var currentBytes = Encoding.UTF8.GetBytes(currentVersion);
            var receivedBytes = Encoding.UTF8.GetBytes(passwordVersion);

            return currentBytes.Length == receivedBytes.Length &&
                   CryptographicOperations.FixedTimeEquals(currentBytes, receivedBytes);
        }

        private string CreatePasswordVersion(string passwordHash)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_key));
            return Base64UrlEncoder.Encode(hmac.ComputeHash(Encoding.UTF8.GetBytes(passwordHash)));
        }
    }
}
