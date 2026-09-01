using f_backend_gestafe.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace f_backend_gestafe.Data.Builders
{
    public class EscalaBuilder
    {
        public static void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Escala>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.IgrejaId)
                    .IsRequired();

                entity.Property(e => e.DataSalvamento)
                    .IsRequired();

                entity.Property(e => e.HoraSalvamento)
                    .IsRequired();

                entity.Property(e => e.EscalaJson)
                    .IsRequired().HasColumnName("configuracoes").HasColumnType("jsonb"); ;

                entity.HasOne(e => e.Igreja)
                    .WithMany()
                    .HasForeignKey(e => e.IgrejaId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}