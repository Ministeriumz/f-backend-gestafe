using AutoMapper;
using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Objects.Models;
using f_backend_gestafe.Services.Interfaces;

namespace f_backend_gestafe.Services.Entities
{
    public class ConfiguracoesService : GenericService<Configuracoes, ConfiguracoesDTO>, IConfiguracoesService
    {
        private readonly IConfiguracoesRepository _configuracoesRepository;
        private readonly IMapper _mapper;

        public ConfiguracoesService(IConfiguracoesRepository configuracoesRepository, IMapper mapper) : base(configuracoesRepository, mapper)
        {
            _configuracoesRepository = configuracoesRepository;
            _mapper = mapper;

        }
    }
}
