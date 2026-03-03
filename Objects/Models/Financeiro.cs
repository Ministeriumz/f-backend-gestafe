using f_backend_gestafe.Objects.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace f_backend_gestafe.Objects.Models
{
    [Table("financeiro")]
    public class Financeiro
    {
        [Key]
        [Required]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("valor")]
        public decimal Valor { get; set; }

        [Required]
        [Column("acao")]
        public string Acao { get; set; }

        [Required]
        [Column("data")]
        public DateTime Data { get; set; }

        [Required]
        [Column("status")]
        public StatusFinanceiro Status { get; set; }

        [ForeignKey("IgrejaId")]
        public int? IgrejaId { get; set; }
        public Igreja Igreja { get; set; }

    }
}
