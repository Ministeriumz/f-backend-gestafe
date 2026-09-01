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

        public async Task<CargosUsuario> GetByUsuarioECargoAsync(int idUsuario, int idCargo)
        {
            return await _context.Set<CargosUsuario>()
                .FirstOrDefaultAsync(cu => cu.IdUsuario == idUsuario && cu.IdCargo == idCargo);
        }

        public async Task<List<CargosUsuario>> ObterTodosComRelacionamentosAsync()
        {
            // Apenas busca os dados, sem regras de negócio
            return await _context.CargosUsuarios
                .Include(cu => cu.Usuario)
                .Include(cu => cu.Cargo)
                .ToListAsync();
        }
    }
}