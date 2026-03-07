using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Objects.Models;

namespace f_backend_gestafe.Data.Repositories
{
    public class CargosRepository : GenericRepository<Cargo>, ICargosRepository
    {
        private readonly AppDbContext _context;
        public CargosRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
