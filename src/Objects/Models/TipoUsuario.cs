using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace f_backend_gestafe.src.Objects.Models
{
    [Table("tipo_usuario")]
    public class TipoUsuario
    {
        [Key]
        [Required]
        [Column("id_tipo")]
        public int Id { get; set; }

        [Required]
        [Column("nome")]
        [MaxLength(100)]
        public string Nome { get; set; }

        public ICollection<Usuario> Usuarios { get; set; }
    }
}
