using f_backend_gestafe.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace f_backend_gestafe.Data.Builders;

public class ConfiguracoesBuilder
{
    public static void Build(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Configuracoes>().HasKey(c => c.IgrejaId);
        modelBuilder.Entity<Configuracoes>().Property(c => c.IgrejaId).HasColumnName("igreja_id");
        modelBuilder.Entity<Configuracoes>().Property(c => c.ConfiguracaoJson).IsRequired().HasColumnName("configuracoes").HasColumnType("jsonb");
        modelBuilder.Entity<Configuracoes>().HasOne(c => c.Igreja).WithOne(i => i.Configuracoes).HasForeignKey<Configuracoes>(c => c.IgrejaId).OnDelete(DeleteBehavior.Cascade);
    }
}