using AutoMapper;
using f_backend_gestafe.Data.Interfaces;
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
    }
}