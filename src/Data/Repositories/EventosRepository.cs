using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Objects.Models;

namespace f_backend_gestafe.Data.Repositories;

public class EventosRepository : GenericRepository<Eventos>, IEventosRepository
{
    private readonly AppDbContext _context;

    public EventosRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}