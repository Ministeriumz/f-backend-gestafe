using f_backend_gestafe.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace f_backend_gestafe.Data.Builders
{
    public class MinisterioBuilder
    {
        public static void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ministerio>().HasKey(t => t.Id);

            modelBuilder.Entity<Ministerio>().Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Ministerio>().Property(t => t.Tamanho_max);
        }
    }
}
