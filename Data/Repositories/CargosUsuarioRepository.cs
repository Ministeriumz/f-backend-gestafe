using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace f_backend_gestafe.Data.Repositories
{
    public class CargosUsuarioRepository : GenericRepository<CargosUsuario>, ICargosUsuarioRepository
    {
        private readonly AppDbContext _context;

        public CargosUsuarioRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<CargosUsuario> GetByCompositeId(int usuarioId, int cargoId)
        {
            return await _context.CargosUsuarios.FirstOrDefaultAsync(x => x.IdUsuario == usuarioId && x.IdCargo == cargoId);
        }

        public async Task RemoveComposite(int usuarioId, int cargoId)
        {
            var entity = await GetByCompositeId(usuarioId, cargoId);

            if (entity != null)
            {
                _context.CargosUsuarios.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}