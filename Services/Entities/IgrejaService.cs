using AutoMapper;
using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Objects.Models;
using f_backend_gestafe.Services.Interfaces;

namespace f_backend_gestafe.Services.Entities
{
    public class IgrejaService : GenericService<Igreja, IgrejaDTO>, IIgrejaService
    {
        private readonly IIgrejaRepository _igrejaRepository;
        private readonly IMapper _mapper;

        public IgrejaService(IIgrejaRepository igrejaRepository, IMapper mapper) : base(igrejaRepository, mapper)
        {
            _igrejaRepository = igrejaRepository;
            _mapper = mapper;
        }
    }
}
