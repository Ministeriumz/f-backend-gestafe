using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Objects.Models;

namespace f_backend_gestafe.Data.Repositories
{
    public class EscalaRepository : GenericRepository<Escala>, IEscalaRepository
    {
        private readonly AppDbContext _context;
        public EscalaRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
