using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace f_backend_gestafe.Data.Repositories
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByEmailAndSenha(string email, string senha)
        {
            return await _context.Usuario.FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha);
        }
    }
}
