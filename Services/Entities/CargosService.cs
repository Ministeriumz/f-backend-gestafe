using AutoMapper;
using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Middleware.Exceptions;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Objects.Models;
using f_backend_gestafe.Services.Interfaces;

namespace f_backend_gestafe.Services.Entities
{
    public class CargosService : GenericService<Cargo, CargosDTO>, ICargosService
    {
        private readonly ICargosRepository _cargosRepository;
        private readonly IMapper _mapper;

        public CargosService(ICargosRepository cargosRepository, IMapper mapper) : base(cargosRepository, mapper)
        {
            _cargosRepository = cargosRepository;
            _mapper = mapper;
        }

        public override async Task Create(CargosDTO entityDTO)
        {
            var errors = new List<string>();

            if (entityDTO is null)
            {
                errors.Add("O objeto não pode ser nulo.");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(entityDTO.Nome))
                {
                    errors.Add("O campo 'Nome' é obrigatório.");
                }

                if (entityDTO.Nome.Length > 100)
                {
                    errors.Add("O campo 'Nome' deve ter no máximo 100 caracteres.");
                }
            }

            if (errors.Any())
            {
                throw new ValidationException(errors);
            }

            var entity = _mapper.Map<Cargo>(entityDTO);
            await _cargosRepository.Add(entity);
        }
    }
}