using System.ComponentModel.DataAnnotations;

namespace f_backend_gestafe.Objects.Dtos.Entities
{
    public class PasswordResetTokenRequestDTO
    {
        [Required(ErrorMessage = "O token é obrigatório.")]
        public string Token { get; set; } = string.Empty;
    }
}
