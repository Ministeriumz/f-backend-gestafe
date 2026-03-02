using AutoMapper;
using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Data.Repositories;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Objects.Models;
using f_backend_gestafe.Services.Interfaces;

namespace f_backend_gestafe.Services.Entities
{
    public class MinisterioService : GenericService<Ministerio, MinisterioDTO>, IMinisterioService
    {
        private readonly IMinisterioRepository _ministerioRepository;
        private readonly IMapper _mapper;

        public MinisterioService( IMinisterioRepository ministerioRepository, IMapper mapper) : base(ministerioRepository, mapper)
        {
            _ministerioRepository = ministerioRepository;
            _mapper = mapper;
        }
    }
}
