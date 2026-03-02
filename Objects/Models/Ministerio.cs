using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace f_backend_gestafe.Objects.Models
{
    [Table("ministerio")]
    public class Ministerio
    {
        [Key]
        [Required]
        [Column("id_ministerio")]
        public int Id { get; set; }

        [Required]
        [Column("nome")]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        
        [Column("tamanho_max")]
        public int Tamanho_max { get; set; }

    }
}
