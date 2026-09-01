using f_backend_gestafe.Configurations;
using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Objects.Contracts;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Objects.Models;
using f_backend_gestafe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using f_backend_gestafe.Services.Security;
using f_backend_gestafe.Objects.Authorization;

namespace f_backend_gestafe.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogService _logService;
        private readonly IWhatsAppIntegrationService _whatsAppService;
        private readonly IPasswordResetTokenService _passwordResetTokenService;
        private readonly PasswordResetSettings _passwordResetSettings;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;
        private readonly Response _response;

        public AuthController(
            IUsuarioRepository usuarioRepository,
            ILogService logService,
            IWhatsAppIntegrationService whatsAppService,
            IPasswordResetTokenService passwordResetTokenService,
            IOptions<PasswordResetSettings> passwordResetSettings,
            ILogger<AuthController> logger,
            IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _logService = logService;
            _whatsAppService = whatsAppService;
            _passwordResetTokenService = passwordResetTokenService;
            _passwordResetSettings = passwordResetSettings.Value;
            _logger = logger;
            _configuration = configuration;
            _response = new Response();
        }

        [AllowAnonymous]
        [EnableRateLimiting("login")]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {

            if (request is null)
            {
                _response.Code = ResponseEnum.INVALID;
                _response.Message = "Dados inválidos";
                _response.Data = null;
                return BadRequest(_response);
            }

            var usuario = await _usuarioRepository.GetByEmail(request.Email);

            if (usuario is null || !PasswordHasher.Verify(request.Senha, usuario.Senha))
            {
                _response.Code = ResponseEnum.UNAUTHORIZED;
                _response.Message = "E-mail ou senha inválidos";
                _response.Data = null;
                return Unauthorized(_response);
            }

            var (token, expiracao) = GerarToken(usuario);

            await _logService.Create(new LogDTO
            {
                Data = DateTime.UtcNow.Date,
                Hora = DateTime.UtcNow.TimeOfDay,
                Acao = "LOGIN",
                IdUsuario = usuario.Id
            });

            _response.Code = ResponseEnum.SUCCESS;
            _response.Message = "Login realizado com sucesso";
            _response.Data = new LoginResponseDTO
            {
                Token = token,
                ExpiraEm = expiracao,
                UsuarioId = usuario.Id,
                Email = usuario.Email
            };

            return Ok(_response);
        }

        [AllowAnonymous]
        [EnableRateLimiting("password-reset")]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDTO request)
        {
            const string genericMessage = "Se o e-mail estiver cadastrado, você receberá um link de recuperação pelo WhatsApp.";
            var usuario = await _usuarioRepository.GetByEmail(request.Email);

            if (usuario is not null)
            {
                try
                {
                    var (token, _) = _passwordResetTokenService.Create(usuario);
                    var resetUrl = BuildResetUrl(token);
                    var phone = NormalizePhone(usuario.Telefone);

                    if (phone is null)
                    {
                        _logger.LogWarning("Não foi possível enviar a recuperação de senha: telefone inválido para o usuário {UserId}.", usuario.Id);
                    }
                    else
                    {
                        var message = $"GestaFé: redefina sua senha em até {_passwordResetSettings.ExpiresMinutes} min:\n{resetUrl}\nSe não solicitou, ignore.";

                        if (message.Length > 500)
                        {
                            _logger.LogError("A mensagem de recuperação excedeu 500 caracteres ({Length}) para o usuário {UserId}.", message.Length, usuario.Id);
                            return GenericForgotPasswordResponse(genericMessage);
                        }

                        var sent = await _whatsAppService.SendMessageAsync(new SendWhatsAppMessageRequestDTO
                        {
                            Phone = phone,
                            Text = message
                        }, HttpContext.RequestAborted);

                        if (!sent)
                        {
                            _logger.LogWarning("O serviço de WhatsApp recusou a recuperação de senha do usuário {UserId}.", usuario.Id);
                        }
                    }
                }
                catch (Exception exception) when (exception is not OperationCanceledException)
                {
                    _logger.LogError(exception, "Falha ao enviar a recuperação de senha do usuário {UserId}.", usuario.Id);
                }
            }

            return GenericForgotPasswordResponse(genericMessage);
        }

        [AllowAnonymous]
        [HttpPost("reset-password/validate")]
        public async Task<IActionResult> ValidateResetPasswordToken([FromBody] PasswordResetTokenRequestDTO request)
        {
            var usuario = await GetUserFromResetToken(request.Token);
            if (usuario is null)
            {
                return InvalidResetTokenResponse();
            }

            _response.Code = ResponseEnum.SUCCESS;
            _response.Message = "Link de recuperação válido.";
            _response.Data = null;
            return Ok(_response);
        }

        [AllowAnonymous]
        [EnableRateLimiting("password-reset")]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDTO request)
        {
            if (request.NovaSenha != request.ConfirmacaoSenha)
            {
                _response.Code = ResponseEnum.INVALID;
                _response.Message = "As senhas não coincidem.";
                _response.Data = null;
                return BadRequest(_response);
            }

            if (!PasswordPolicy.IsValid(request.NovaSenha))
            {
                _response.Code = ResponseEnum.INVALID;
                _response.Message = PasswordPolicy.ErrorMessage;
                _response.Data = null;
                return BadRequest(_response);
            }

            var usuario = await GetUserFromResetToken(request.Token);
            if (usuario is null)
            {
                return InvalidResetTokenResponse();
            }

            usuario.Senha = PasswordHasher.Hash(request.NovaSenha);
            await _usuarioRepository.Update(usuario);

            try
            {
                await _logService.Create(new LogDTO
                {
                    Data = DateTime.UtcNow.Date,
                    Hora = DateTime.UtcNow.TimeOfDay,
                    Acao = "RESET_PASSWORD",
                    IdUsuario = usuario.Id
                });
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, "A senha do usuário {UserId} foi redefinida, mas não foi possível registrar o log.", usuario.Id);
            }

            _response.Code = ResponseEnum.SUCCESS;
            _response.Message = "Senha redefinida com sucesso.";
            _response.Data = null;
            return Ok(_response);
        }

        private async Task<Usuario?> GetUserFromResetToken(string token)
        {
            var tokenData = _passwordResetTokenService.Validate(token);
            if (tokenData is null)
            {
                return null;
            }

            var usuario = await _usuarioRepository.GetById(tokenData.UserId);
            return usuario is not null && _passwordResetTokenService.IsCurrent(usuario, tokenData.PasswordVersion)
                ? usuario
                : null;
        }

        private IActionResult InvalidResetTokenResponse()
        {
            _response.Code = ResponseEnum.INVALID;
            _response.Message = "O link de recuperação é inválido, expirou ou já foi utilizado.";
            _response.Data = null;
            return BadRequest(_response);
        }

        private IActionResult GenericForgotPasswordResponse(string message)
        {
            _response.Code = ResponseEnum.SUCCESS;
            _response.Message = message;
            _response.Data = null;
            return Ok(_response);
        }

        private string BuildResetUrl(string token)
        {
            var frontendUrl = string.IsNullOrWhiteSpace(_passwordResetSettings.FrontendUrl)
                ? "http://localhost:3000/redefinir-senha"
                : _passwordResetSettings.FrontendUrl.Trim();
            var separator = frontendUrl.Contains('?') ? "&" : "?";
            return $"{frontendUrl}{separator}token={Uri.EscapeDataString(token)}";
        }

        private static string? NormalizePhone(string phone)
        {
            var digits = Regex.Replace(phone ?? string.Empty, @"\D", string.Empty);
            if (digits.Length is 10 or 11)
            {
                digits = $"55{digits}";
            }

            return digits.Length is >= 10 and <= 15 ? digits : null;
        }

        private (string token, DateTime expiracao) GerarToken(Usuario usuario)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Chave JWT não configurada.");
            var issuer = _configuration["Jwt:Issuer"] ?? "f-backend-gestafe";
            var audience = _configuration["Jwt:Audience"] ?? "f-backend-gestafe-client";
            var expiresMinutes = int.TryParse(_configuration["Jwt:ExpiresMinutes"], out var value) ? value : 60;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiracao = DateTime.UtcNow.AddMinutes(expiresMinutes);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(AuthorizationClaimTypes.UserTypeId, usuario.IdTipoUsuario.ToString()),
                new Claim(AuthorizationClaimTypes.AccessLevel, ((int)usuario.TipoUsuario.NivelAcesso).ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiracao,
                signingCredentials: creds);

            return (new JwtSecurityTokenHandler().WriteToken(token), expiracao);
        }
    }
}
