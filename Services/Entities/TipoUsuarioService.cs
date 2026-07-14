using AutoMapper;
using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Data.Repositories;
using f_backend_gestafe.Middleware.Exceptions;
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

        public override async Task Create(TipoUsuarioDTO entityDTO)
        {
            var errors = new List<string>();

            if (entityDTO is null)
            {
                errors.Add("O objeto não pode ser nulo.");
            }
            else
            {
                // Validação do campo Nome
                if (string.IsNullOrWhiteSpace(entityDTO.Nome))
                {
                    errors.Add("O campo 'Nome' é obrigatório.");
                }
                else if (entityDTO.Nome.Length > 100)
                {
                    errors.Add("O campo 'Nome' deve ter no máximo 100 caracteres.");
                }
            }

            if (errors.Any())
            {
                throw new ValidationException(errors);
            }

            var entity = _mapper.Map<TipoUsuario>(entityDTO);
            await _tipoUsuarioRepository.Add(entity);
        }
    }
}
