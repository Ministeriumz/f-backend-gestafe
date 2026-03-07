using f_backend_gestafe.Objects.Models;

namespace f_backend_gestafe.Data.Interfaces
{
    public interface ICargosUsuarioRepository : IGenericRepository<CargosUsuario>
    {
        Task<CargosUsuario> GetByCompositeId(int usuarioId, int cargoId);
        Task RemoveComposite(int usuarioId, int cargoId);
    }
}