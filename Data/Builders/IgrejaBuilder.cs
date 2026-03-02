using f_backend_gestafe.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace f_backend_gestafe.Data.Builders
{
    public class IgrejaBuilder
    {
        public static void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Igreja>().HasKey(i => i.Id);
            modelBuilder.Entity<Igreja>().Property(i => i.Nome).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Igreja>().Property(i => i.Cnpj).IsRequired() .HasMaxLength(18);
            modelBuilder.Entity<Igreja>().Property(i => i.Estado).IsRequired().HasMaxLength(2);
            modelBuilder.Entity<Igreja>().Property(i => i.Rua).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Igreja>().Property(i => i.Cep).IsRequired().HasMaxLength(10);
            modelBuilder.Entity<Igreja>().Property(i => i.Numero).IsRequired().HasMaxLength(20);
        }
    }
}
