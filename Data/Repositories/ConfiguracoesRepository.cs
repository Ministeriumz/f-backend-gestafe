using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Objects.Models;

namespace f_backend_gestafe.Data.Repositories
{
    public class ConfiguracoesRepository : GenericRepository<Configuracoes>, IConfiguracoesRepository
    {
        private readonly AppDbContext _context;
        public ConfiguracoesRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
