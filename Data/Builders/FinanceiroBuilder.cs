using f_backend_gestafe.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace f_backend_gestafe.Data.Builders
{
    public class FinanceiroBuilder
    {
        public static void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Financeiro>().HasKey(i => i.Id);
            modelBuilder.Entity<Financeiro>().Property(i => i.Valor).IsRequired();
            modelBuilder.Entity<Financeiro>().Property(i => i.Acao).IsRequired();
            modelBuilder.Entity<Financeiro>().Property(i => i.Data).IsRequired();
            modelBuilder.Entity<Financeiro>().Property(i => i.Status).IsRequired();
        }
    }
}
