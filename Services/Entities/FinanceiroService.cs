using AutoMapper;
using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Objects.Models;
using f_backend_gestafe.Services.Interfaces;

namespace f_backend_gestafe.Services.Entities
{
    public class FinanceiroService : GenericService<Financeiro, FinanceiroDTO>, IFinanceiroService
    {
        private readonly IFinanceiroRepository _financeiroRepository;
        private readonly IMapper _mapper;

        public FinanceiroService(IFinanceiroRepository financeiroRepository, IMapper mapper) : base(financeiroRepository, mapper)
        {
            _financeiroRepository = financeiroRepository;
            _mapper = mapper;
        }
    }
}
