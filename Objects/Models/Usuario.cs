using f_backend_gestafe.Objects.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace f_backend_gestafe.Objects.Models
{
    [Table("usuario")]
    public class Usuario
    {
        [Key]
        [Required]
        [Column("id_usuario")]
        public int Id { get; set; }

        [Required]
        [Column("nome")]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required]
        [Column("sobrenome")]
        [MaxLength(100)]
        public string Sobrenome { get; set; }

        [Required]
        [Column("telefone")]
        [MaxLength(18)]
        public string Telefone { get; set; }

        [Required]
        [Column("email")]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Column("senha")]
        [MaxLength(50)]
        public string Senha { get; set; }

        // Chaves estrangeiras
        [Column("id_igreja")]
        public int IdIgreja { get; set; }

        [ForeignKey("IdIgreja")]
        public Igreja Igreja { get; set; }

        [Column("id_tipo")]
        public int IdTipoUsuario { get; set; }

        [ForeignKey("IdTipoUsuario")]
        public TipoUsuario TipoUsuario { get; set; }
    }
}
