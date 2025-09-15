using f_backend_gestafe.src.Objects.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace f_backend_gestafe.Objects.Models
{
    [Table("igreja")]
    public class Igreja
    {
        [Key]
        [Required]
        [Column("id")]
        public int Id { get; set; }

        [Column("nome")]
        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Column("cnpj")]
        [Required]
        [MaxLength(18)]
        public string Cnpj { get; set; }

        [Column("estado")]
        [MaxLength(2)]
        [Required]
        public string Estado { get; set; }

        [Column("rua")]
        [MaxLength(100)]
        [Required]
        public string Rua { get; set; }

        [Column("cep")]
        [MaxLength(10)]
        [Required]
        public string Cep { get; set; }

        [Column("numero")]
        [MaxLength(20)]
        [Required]
        public string Numero { get; set; }

        public ICollection<Usuario> Usuarios { get; set; }
    }
}
