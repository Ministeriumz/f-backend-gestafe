using f_backend_gestafe.Data;
using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Data.Repositories;
using f_backend_gestafe.src.Data.Interfaces;
using f_backend_gestafe.src.Objects.Models;
using f_backend_gestafe.src.Services.Interfaces;

namespace f_backend_gestafe.src.Data.Repositories
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
