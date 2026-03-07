using AutoMapper;
using f_backend_gestafe.Data.Interfaces;
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
    }
}