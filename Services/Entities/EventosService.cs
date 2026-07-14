using AutoMapper;
using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Middleware.Exceptions;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Objects.Models;
using f_backend_gestafe.Services.Interfaces;

namespace f_backend_gestafe.Services.Entities
{
    public class EventosService : GenericService<Eventos, EventosDTO>, IEventosService
    {
        private readonly IEventosRepository _eventosRepository;
        private readonly IMapper _mapper;

        public EventosService(IEventosRepository eventosRepository, IMapper mapper) : base(eventosRepository, mapper)
        {
            _eventosRepository = eventosRepository;
            _mapper = mapper;
        }

        public override async Task Create(EventosDTO entityDTO)
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

                // Validação do campo Tipo
                if (string.IsNullOrWhiteSpace(entityDTO.Tipo))
                {
                    errors.Add("O campo 'Tipo' é obrigatório.");
                }
                else if (entityDTO.Tipo.Length > 100)
                {
                    errors.Add("O campo 'Tipo' deve ter no máximo 100 caracteres.");
                }

                // Validação do campo Resumo
                if (string.IsNullOrWhiteSpace(entityDTO.Resumo))
                {
                    errors.Add("O campo 'Resumo' é obrigatório.");
                }
                else if (entityDTO.Resumo.Length > 100)
                {
                    errors.Add("O campo 'Resumo' deve ter no máximo 100 caracteres.");
                }

                // Validação da Data
                if (entityDTO.Data == default)
                {
                    errors.Add("O campo 'Data' é obrigatório e deve ser uma data válida.");
                }

                // Validação de coerência das Horas
                if (entityDTO.Hora_inicio == default && entityDTO.Hora_fim == default)
                {
                    errors.Add("Os campos 'Hora_inicio' e 'Hora_fim' devem ser informados.");
                }
                else if (entityDTO.Hora_inicio >= entityDTO.Hora_fim)
                {
                    errors.Add("A 'Hora_inicio' não pode ser maior ou igual à 'Hora_fim'.");
                }
            }

            if (errors.Any())
            {
                throw new ValidationException(errors);
            }

            var entity = _mapper.Map<Eventos>(entityDTO);
            await _eventosRepository.Add(entity);
        }
    }
}