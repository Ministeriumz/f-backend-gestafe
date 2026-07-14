using AutoMapper;
using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Middleware.Exceptions;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Objects.Models;
using f_backend_gestafe.Services.Interfaces;

namespace f_backend_gestafe.Services.Entities
{
    public class EscalaService : GenericService<Escala, EscalaDTO>, IEscalaService
    {
        private readonly IEscalaRepository _escalaRepository;
        private readonly ICargosRepository _cargosRepository;
        private readonly IMapper _mapper;

        public EscalaService(
            IEscalaRepository escalaRepository,
            ICargosRepository cargosRepository,
            IMapper mapper) : base(escalaRepository, mapper)
        {
            _escalaRepository = escalaRepository;
            _cargosRepository = cargosRepository;
            _mapper = mapper;
        }

        public override async Task Create(EscalaDTO entityDTO)
        {
            var errors = new List<string>();

            if (entityDTO is null)
            {
                errors.Add("O objeto não pode ser nulo.");
            }
            else
            {
                // Validação do campo Data
                if (entityDTO.Data == default)
                {
                    errors.Add("O campo 'Data' é obrigatório e deve ser uma data válida.");
                }

                // Validação de coerência das Horas
                if (entityDTO.HoraInicio == default && entityDTO.HoraFim == default)
                {
                    // Dependendo do seu sistema, 00:00 pode ser válido. Se for, remova esta checagem específica de default.
                    errors.Add("Os campos 'HoraInicio' e 'HoraFim' devem ser informados.");
                }
                else if (entityDTO.HoraInicio >= entityDTO.HoraFim)
                {
                    errors.Add("A 'HoraInicio' não pode ser maior ou igual à 'HoraFim'.");
                }

                // Validação e verificação de existência do Cargo
                if (entityDTO.CargoId <= 0)
                {
                    errors.Add("O campo 'CargoId' é obrigatório e deve ser um ID válido maior que zero.");
                }
                else
                {
                    var cargoExiste = await _cargosRepository.GetById(entityDTO.CargoId);
                    if (cargoExiste is null)
                    {
                        errors.Add("O Cargo informado não está cadastrado no banco de dados.");
                    }
                }
            }

            if (errors.Any())
            {
                throw new ValidationException(errors);
            }

            var entity = _mapper.Map<Escala>(entityDTO);
            await _escalaRepository.Add(entity);
        }
    }
}
