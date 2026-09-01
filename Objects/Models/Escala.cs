using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace f_backend_gestafe.Objects.Models
{
    [Table("escala")]
    public class Escala
    {
        [Key]
        [Required]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("igreja_id")]
        public int IgrejaId { get; set; }

        [Required]
        [Column("data_salvamento")]
        public DateOnly DataSalvamento { get; set; }

        [Required]
        [Column("hora_salvamento")]
        public TimeOnly HoraSalvamento { get; set; }

        [Required]
        [Column("escala_json", TypeName = "jsonb")]
        public string EscalaJson { get; set; }

        [ForeignKey(nameof(IgrejaId))]
        public Igreja Igreja { get; set; } = null;


    }
}
