using System.ComponentModel.DataAnnotations;

namespace f_backend_gestafe.Objects.Dtos.Entities
{
    public class ForgotPasswordRequestDTO
    {
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        public string Email { get; set; } = string.Empty;
    }
}
