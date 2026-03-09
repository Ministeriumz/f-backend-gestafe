using AutoMapper;
using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Objects.Models;
using f_backend_gestafe.Services.Interfaces;

namespace f_backend_gestafe.Services.Entities
{
    public class EscalaService : GenericService<Escala, EscalaDTO>, IEscalaService
    {
        private readonly IEscalaRepository _escalaRepository;
        private readonly IMapper _mapper;

        public EscalaService(IEscalaRepository escalaRepository, IMapper mapper) : base(escalaRepository, mapper)
        {
            _escalaRepository = escalaRepository;
            _mapper = mapper;
        }
    }
}
