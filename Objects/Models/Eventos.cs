using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace f_backend_gestafe.Objects.Models
{
    [Table("eventos")]
    public class Eventos
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nome")]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Column("tipo")]
        [MaxLength(100)]
        public string Tipo { get; set; }

        [Column("resumo")]
        [MaxLength(255)]
        public string Resumo { get; set; }

        [Column("data")]
        public DateOnly Data { get; set; }

        [Column("hora_inicio")]
        public TimeOnly Hora_inicio { get; set; }

        [Column("hora_fim")]
        public TimeOnly Hora_fim { get; set; }
    }
}
