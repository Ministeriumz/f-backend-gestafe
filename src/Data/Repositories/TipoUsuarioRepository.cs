using f_backend_gestafe.Data;
using f_backend_gestafe.Data.Repositories;
using f_backend_gestafe.src.Data.Interfaces;
using f_backend_gestafe.src.Objects.Models;

namespace f_backend_gestafe.src.Data.Repositories
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
