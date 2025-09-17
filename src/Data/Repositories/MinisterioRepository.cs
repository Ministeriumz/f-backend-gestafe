using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Objects.Models;

namespace f_backend_gestafe.Data.Repositories
{
    public class MinisterioRepository : GenericRepository<Ministerio>, IMinisterioRepository
    {
        private readonly AppDbContext _context;

        public MinisterioRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
