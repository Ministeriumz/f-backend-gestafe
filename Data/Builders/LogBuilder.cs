using Microsoft.EntityFrameworkCore;
using f_backend_gestafe.Objects.Models;

namespace f_backend_gestafe.Data.Builders;

public class LogBuilder
{
    public static void Build(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Log>().HasKey(i => i.Id);
        modelBuilder.Entity<Log>().Property(i => i.Data).IsRequired();
        modelBuilder.Entity<Log>().Property(i => i.Hora).IsRequired();
        modelBuilder.Entity<Log>().Property(i => i.Acao).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<Log>().Property(i => i.IdUsuario).IsRequired(false);
    }
}