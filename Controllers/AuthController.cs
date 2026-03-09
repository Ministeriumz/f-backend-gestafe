using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Objects.Contracts;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Objects.Models;
using f_backend_gestafe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace f_backend_gestafe.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogService _logService;
        private readonly IConfiguration _configuration;
        private readonly Response _response;

        public AuthController(IUsuarioRepository usuarioRepository, ILogService logService, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _logService = logService;
            _configuration = configuration;
            _response = new Response();
        }

        [AllowAnonymous]
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

            var usuario = await _usuarioRepository.GetByEmailAndSenha(request.Email, request.Senha);

            if (usuario is null)
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
