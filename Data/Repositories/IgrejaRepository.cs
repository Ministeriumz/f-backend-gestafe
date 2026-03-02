using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Objects.Models;

namespace f_backend_gestafe.Data.Repositories
{
    public class IgrejaRepository : GenericRepository<Igreja>, IIgrejaRepository
    {
        private readonly AppDbContext _context;

        public IgrejaRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
