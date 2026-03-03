using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace f_backend_gestafe.Objects.Models
{
    [Table("configuracoes")]
    public class Configuracoes
    {
        [Key]
        [Column("igreja_id")]
        public int IgrejaId { get; set; }

        public Igreja Igreja { get; set; }

        [Required]
        [Column("configuracoes", TypeName = "jsonb")]
        public string ConfiguracaoJson { get; set; }
    }
}
