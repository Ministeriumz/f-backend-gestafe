using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Column("data")]
        public DateOnly Data { get; set; }

        [Required]
        [Column("hora_inicio")]
        public TimeOnly HoraInicio { get; set; }

        [Required]
        [Column("hora_fim")]
        public TimeOnly HoraFim { get; set; }

        [ForeignKey("CargoId")]
        public int CargoId { get; set; }
        public Cargo Cargo { get; set; }

    }
}
