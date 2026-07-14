using AutoMapper;
using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Middleware.Exceptions;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Objects.Enums;
using f_backend_gestafe.Objects.Models;
using f_backend_gestafe.Services.Interfaces;

namespace f_backend_gestafe.Services.Entities
{
    public class FinanceiroService : GenericService<Financeiro, FinanceiroDTO>, IFinanceiroService
    {
        private readonly IFinanceiroRepository _financeiroRepository;
        private readonly IIgrejaRepository _igrejaRepository; // Injetando para validar a existência da Igreja
        private readonly IMapper _mapper;

        public FinanceiroService(
            IFinanceiroRepository financeiroRepository,
            IIgrejaRepository igrejaRepository,
            IMapper mapper) : base(financeiroRepository, mapper)
        {
            _financeiroRepository = financeiroRepository;
            _igrejaRepository = igrejaRepository;
            _mapper = mapper;
        }

        public override async Task Create(FinanceiroDTO entityDTO)
        {
            var errors = new List<string>();

            if (entityDTO is null)
            {
                errors.Add("O objeto não pode ser nulo.");
            }
            else
            {
                // Validação do campo Valor
                if (entityDTO.Valor <= 0) // Considerando que transações devem ter valor maior que zero
                {
                    errors.Add("O campo 'Valor' é obrigatório e deve ser maior que zero.");
                }

                // Validação do campo Acao
                if (string.IsNullOrWhiteSpace(entityDTO.Acao))
                {
                    errors.Add("O campo 'Acao' é obrigatório.");
                }

                // Validação do campo Data
                if (entityDTO.Data == default)
                {
                    errors.Add("O campo 'Data' é obrigatório e deve ser uma data válida.");
                }

                // Validação do Enum de Status
                if (!Enum.IsDefined(typeof(StatusFinanceiro), entityDTO.Status))
                {
                    errors.Add("O 'Status' informado é inválido. Utilize valores correspondentes (ex: 1 para Pago, 2 para Pendente).");
                }

                // Validação de existência da Igreja (Tratando como nullable conforme sua Model)
                
                if (entityDTO.IgrejaId <= 0)
                {
                    errors.Add("O campo 'IgrejaId' deve ser um ID válido.");
                }
                else
                {
                    var igrejaExiste = await _igrejaRepository.GetById(entityDTO.IgrejaId);
                    if (igrejaExiste is null)
                    {
                        errors.Add("A Igreja informada não está cadastrada no banco de dados.");
                    }
                }
                
            }

            if (errors.Any())
            {
                throw new ValidationException(errors);
            }

            var entity = _mapper.Map<Financeiro>(entityDTO);
            await _financeiroRepository.Add(entity);
        }
    }
}
