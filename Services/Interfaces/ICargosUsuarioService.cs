using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Objects.Models;

namespace f_backend_gestafe.Services.Interfaces
{
    public interface ICargosUsuarioService : IGenericService<CargosUsuario, CargosUsuarioDTO>
    {
        Task<CargosUsuarioDTO> GetByCompositeId(int usuarioId, int cargoId);
        Task RemoveComposite(int usuarioId, int cargoId);
    }
}