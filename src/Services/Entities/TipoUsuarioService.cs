using AutoMapper;
using f_backend_gestafe.Services.Entities;
using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Objects.Models;
using f_backend_gestafe.Services.Interfaces;

namespace f_backend_gestafe.Services.Entities
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
