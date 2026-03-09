using f_backend_gestafe.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace f_backend_gestafe.Data.Builders
{
    public class EscalaBuilder
    {
        public static void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Escala>().HasKey(e => e.Id);
            modelBuilder.Entity<Escala>().Property(e => e.Data).IsRequired();
            modelBuilder.Entity<Escala>().Property(e => e.HoraInicio).IsRequired();
            modelBuilder.Entity<Escala>().Property(e => e.HoraFim).IsRequired();
        }
    }
}
