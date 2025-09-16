using AutoMapper;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Objects.Models;

namespace f_backend_gestafe.Objects.Dtos.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Igreja, IgrejaDTO>().ReverseMap();
            CreateMap<Eventos, EventosDTO>().ReverseMap();
        }
    }
}
