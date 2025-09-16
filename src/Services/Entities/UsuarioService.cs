using AutoMapper;
using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Objects.Models;
using f_backend_gestafe.Services.Entities;
using f_backend_gestafe.Services.Interfaces;
using f_backend_gestafe.src.Data.Interfaces;
using f_backend_gestafe.src.Data.Repositories;
using f_backend_gestafe.src.Objects.Dtos.Entities;
using f_backend_gestafe.src.Objects.Models;
using f_backend_gestafe.src.Services.Interfaces;

namespace f_backend_gestafe.src.Services.Entities
{
    public class UsuarioService : GenericService<Usuario, UsuarioDTO>, IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public UsuarioService(IUsuarioRepository usuarioRepository, IMapper mapper) : base(usuarioRepository, mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }
    }
}
