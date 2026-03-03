using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Objects.Models;

namespace f_backend_gestafe.Data.Repositories
{
    public class FinanceiroRepository : GenericRepository<Financeiro>, IFinanceiroRepository
    {
        private readonly AppDbContext _context;
        public FinanceiroRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}