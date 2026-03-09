using System.ComponentModel.DataAnnotations;

namespace f_backend_gestafe.Objects.Dtos.Entities
{
    public class LoginRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }
    }
}
