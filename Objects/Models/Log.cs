using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace f_backend_gestafe.Objects.Models
{
    [Table("logs")]
    public class Log
    {
        [Key]
        [Required]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("data")]
        public DateTime Data { get; set; }

        [Required]
        [Column("hora")]
        public TimeSpan Hora { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("acao")]
        public string Acao { get; set; }

        [Column("id_usuario")]
        public int? IdUsuario { get; set; }
    }
}
