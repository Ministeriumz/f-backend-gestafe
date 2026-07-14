using AutoMapper;
using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Middleware.Exceptions;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Objects.Models;
using f_backend_gestafe.Services.Interfaces;

namespace f_backend_gestafe.Services.Entities
{
    public class MinisterioService : GenericService<Ministerio, MinisterioDTO>, IMinisterioService
    {
        private readonly IMinisterioRepository _ministerioRepository;
        private readonly IMapper _mapper;

        public MinisterioService(IMinisterioRepository ministerioRepository, IMapper mapper) : base(ministerioRepository, mapper)
        {
            _ministerioRepository = ministerioRepository;
            _mapper = mapper;
        }

        public override async Task Create(MinisterioDTO entityDTO)
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

                // Validação do campo Tamanho_max
                // Assumindo que o tamanho máximo de um ministério não pode ser negativo
                if (entityDTO.Tamanho_max <= 0)
                {
                    errors.Add("O campo 'Tamanho_max' não pode ser um valor negativo ou igual a zero");
                }
            }

            if (errors.Any())
            {
                throw new ValidationException(errors);
            }

            var entity = _mapper.Map<Ministerio>(entityDTO);
            await _ministerioRepository.Add(entity);
        }
    }
}