using f_backend_gestafe.Objects.Models;
using Microsoft.EntityFrameworkCore;
using f_backend_gestafe.Data.Builders;

namespace f_backend_gestafe.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<Igreja> Igrejas { get; set; }
        public DbSet<Eventos> Eventos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            IgrejaBuilder.Build(modelBuilder);
            EventosBuilder.Build(modelBuilder);
        }
    }
}
