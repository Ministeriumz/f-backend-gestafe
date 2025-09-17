using f_backend_gestafe.Objects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace f_backend_gestafe.Data.Builders
{
    public class TipoUsuarioBuilder
    {
        public static void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TipoUsuario>().HasKey(t => t.Id);

            modelBuilder.Entity<TipoUsuario>().Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
