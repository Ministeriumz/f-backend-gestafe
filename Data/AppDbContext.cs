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
        public DbSet<Ministerio> Ministerios { get; set; }
        public DbSet<TipoUsuario> TipoUsuario { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Configuracoes> Configuracoes { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<Cargo> Cargo { get; set; }
        public DbSet<CargosUsuario> CargosUsuarios { get; set; }
        public DbSet<Escala> Escala { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            IgrejaBuilder.Build(modelBuilder);
            EventosBuilder.Build(modelBuilder);
            MinisterioBuilder.Build(modelBuilder);
            UsuarioBuilder.Build(modelBuilder);
            TipoUsuarioBuilder.Build(modelBuilder);
            ConfiguracoesBuilder.Build(modelBuilder);
            LogBuilder.Build(modelBuilder);
            CargosBuilder.Build(modelBuilder);
            CargosUsuarioBuilder.Build(modelBuilder);
            EscalaBuilder.Build(modelBuilder);

        }
    }
}
