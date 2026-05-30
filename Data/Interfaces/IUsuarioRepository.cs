using f_backend_gestafe.Objects.Models;

namespace f_backend_gestafe.Data.Interfaces
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Task<Usuario?> GetByEmail(string email);
    }
}
