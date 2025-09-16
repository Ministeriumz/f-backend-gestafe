using f_backend_gestafe.Objects.Models;
using f_backend_gestafe.src.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace f_backend_gestafe.src.Data.Builders
{
    public class UsuarioBuilder
    {
        public static void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasKey(u => u.Id);

            modelBuilder.Entity<Usuario>().Property(u => u.Nome)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Usuario>().Property(u => u.Sobrenome)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Usuario>().Property(u => u.Telefone)
                .IsRequired()
                .HasMaxLength(18);

            modelBuilder.Entity<Usuario>().Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Usuario>().Property(u => u.Senha)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Igreja)
                .WithMany(u => u.Usuarios)
                .HasForeignKey(u => u.IdIgreja);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.TipoUsuario)
                .WithMany(u => u.Usuarios)
                .HasForeignKey(u => u.IdTipoUsuario);
        }
    }
}
