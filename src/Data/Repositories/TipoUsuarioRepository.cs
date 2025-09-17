using f_backend_gestafe.Data;
using f_backend_gestafe.Data.Repositories;
using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Objects.Models;

namespace f_backend_gestafe.Data.Repositories
{
    public class TipoUsuarioRepository : GenericRepository<TipoUsuario>, ITipoUsuarioRepository
    {
        private readonly AppDbContext _context;

        public TipoUsuarioRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
