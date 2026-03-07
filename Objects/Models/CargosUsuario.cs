using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace f_backend_gestafe.Objects.Models;

[Table("cargos_usuario")]
public class CargosUsuario
{

    [Key]
    [Column(Order = 1)]
    [ForeignKey(nameof(Usuario))]
    public int IdUsuario { get; set; }

    [Key]
    [Column(Order = 2)]
    [ForeignKey(nameof(Cargo))]
    public int IdCargo { get; set; }

    public Usuario Usuario { get; set; }

    public Cargo Cargo { get; set; }
}