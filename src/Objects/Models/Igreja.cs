using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace f_backend_gestafe.Objects.Models
{
    [Table("igreja")]
    public class Igreja
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nome")]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Column("cnpj")]
        [MaxLength(18)]
        public string Cnpj { get; set; }

        [Column("estado")]
        [MaxLength(2)]
        public string Estado { get; set; }

        [Column("rua")]
        [MaxLength(100)]
        public string Rua { get; set; }

        [Column("cep")]
        [MaxLength(10)]
        public string Cep { get; set; }

        [Column("numero")]
        [MaxLength(20)]
        public string Numero { get; set; }
    }
}
