using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace f_backend_gestafe.Objects.Models;

[Table("cargo")]
public class Cargo
{
    [Key]
    [Required]
    [Column("id_cargo")]
    public int IdCargo { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("nome")]
    public string Nome { get; set; }

    public ICollection<CargosUsuario> CargosUsuario { get; set; }
}