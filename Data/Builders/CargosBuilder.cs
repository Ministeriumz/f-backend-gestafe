using f_backend_gestafe.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace f_backend_gestafe.Data.Builders
{
    public class CargosBuilder
    {
        public static void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cargo>().HasKey(c => c.IdCargo);
            modelBuilder.Entity<Cargo>().Property(c => c.Nome).IsRequired().HasMaxLength(100);
        }
    }
}