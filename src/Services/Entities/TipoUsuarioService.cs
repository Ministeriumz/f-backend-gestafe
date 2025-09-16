using AutoMapper;
using f_backend_gestafe.Services.Entities;
using f_backend_gestafe.src.Data.Interfaces;
using f_backend_gestafe.src.Objects.Dtos.Entities;
using f_backend_gestafe.src.Objects.Models;
using f_backend_gestafe.src.Services.Interfaces;

namespace f_backend_gestafe.src.Services.Entities
{
    public class TipoUsuarioService : GenericService<TipoUsuario, TipoUsuarioDTO>, ITipoUsuarioService
    {
        private readonly ITipoUsuarioRepository _tipoUsuarioRepository;
        private readonly IMapper _mapper;

        public TipoUsuarioService(ITipoUsuarioRepository tipoUsuarioRepository, IMapper mapper) : base(tipoUsuarioRepository, mapper)
        {
            _tipoUsuarioRepository = tipoUsuarioRepository;
            _mapper = mapper;
        }
    }
}
