using f_backend_gestafe.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace f_backend_gestafe.Data.Builders
{
    public class CargosUsuarioBuilder
    {
        public static void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CargosUsuario>().HasKey(x => new { x.IdUsuario, x.IdCargo });
            modelBuilder.Entity<CargosUsuario>().HasOne(x => x.Usuario).WithMany(u => u.CargosUsuario).HasForeignKey(x => x.IdUsuario);
            modelBuilder.Entity<CargosUsuario>().HasOne(x => x.Cargo).WithMany(c => c.CargosUsuario).HasForeignKey(x => x.IdCargo);
        }
    }
}
