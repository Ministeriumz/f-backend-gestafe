using AutoMapper;
using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Objects.Models;
using f_backend_gestafe.Services.Interfaces;

namespace f_backend_gestafe.Services.Entities
{
    public class CargosUsuarioService : GenericService<CargosUsuario, CargosUsuarioDTO>, ICargosUsuarioService
    {
        private readonly ICargosUsuarioRepository _cargosUsuarioRepository;
        private readonly IMapper _mapper;

        public CargosUsuarioService(ICargosUsuarioRepository cargosUsuarioRepository, IMapper mapper)
            : base(cargosUsuarioRepository, mapper)
        {
            _cargosUsuarioRepository = cargosUsuarioRepository;
            _mapper = mapper;
        }

        public async Task<CargosUsuarioDTO> GetByCompositeId(int usuarioId, int cargoId)
        {
            var entity = await _cargosUsuarioRepository.GetByCompositeId(usuarioId, cargoId);

            return _mapper.Map<CargosUsuarioDTO>(entity);
        }

        public async Task RemoveComposite(int usuarioId, int cargoId)
        {
            await _cargosUsuarioRepository.RemoveComposite(usuarioId, cargoId);
        }
    }
}