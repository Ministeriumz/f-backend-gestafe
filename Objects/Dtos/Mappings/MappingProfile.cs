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
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
            CreateMap<TipoUsuario, TipoUsuarioDTO>().ReverseMap();
            CreateMap<Ministerio, MinisterioDTO>().ReverseMap();
            CreateMap<Configuracoes, ConfiguracoesDTO>().ReverseMap();
            CreateMap<Financeiro, FinanceiroDTO>().ReverseMap();
            CreateMap<Log, LogDTO>().ReverseMap();
            CreateMap<Cargo, CargosDTO>().ReverseMap();
            CreateMap<CargosUsuario, CargosUsuarioDTO>().ReverseMap();
            CreateMap<Escala, EscalaDTO>().ReverseMap();
        }
    }
}
