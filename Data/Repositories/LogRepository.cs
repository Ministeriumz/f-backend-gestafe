using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Objects.Models;

namespace f_backend_gestafe.Data.Repositories
{
    public class LogRepository : GenericRepository<Log>, ILogRepository
    {
        private readonly AppDbContext _context;

        public LogRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
