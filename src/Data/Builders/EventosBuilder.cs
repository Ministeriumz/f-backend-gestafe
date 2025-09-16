using f_backend_gestafe.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace f_backend_gestafe.Data.Builders;

public class EventosBuilder
{
    public static void Build(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Eventos>().HasKey(i => i.Id);
        modelBuilder.Entity<Eventos>().Property(i => i.Nome).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<Eventos>().Property(i => i.Tipo).IsRequired() .HasMaxLength(100);
        modelBuilder.Entity<Eventos>().Property(i => i.Resumo).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<Eventos>().Property(i => i.Data).IsRequired();
        modelBuilder.Entity<Eventos>().Property(i => i.Hora_inicio).IsRequired();
        modelBuilder.Entity<Eventos>().Property(i => i.Hora_fim).IsRequired().HasMaxLength(20);
    }
}