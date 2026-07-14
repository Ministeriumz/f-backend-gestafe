using AutoMapper;
using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Middleware.Exceptions;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Objects.Models;
using f_backend_gestafe.Services.Interfaces;
using System.Text.Json; // Necessário para a validação do JSON

namespace f_backend_gestafe.Services.Entities
{
    public class ConfiguracoesService : GenericService<Configuracoes, ConfiguracoesDTO>, IConfiguracoesService
    {
        private readonly IConfiguracoesRepository _configuracoesRepository;
        private readonly IIgrejaRepository _igrejaRepository;
        private readonly IMapper _mapper;

        public ConfiguracoesService(
            IConfiguracoesRepository configuracoesRepository,
            IIgrejaRepository igrejaRepository,
            IMapper mapper) : base(configuracoesRepository, mapper)
        {
            _configuracoesRepository = configuracoesRepository;
            _igrejaRepository = igrejaRepository;
            _mapper = mapper;
        }

        public override async Task Create(ConfiguracoesDTO entityDTO)
        {
            var errors = new List<string>();

            if (entityDTO is null)
            {
                errors.Add("O objeto não pode ser nulo.");
            }
            else
            {
                // Validação e verificação de existência da Igreja
                if (entityDTO.IgrejaId <= 0)
                {
                    errors.Add("O campo 'IgrejaId' é obrigatório e deve ser um ID válido.");
                }
                else
                {
                    var igrejaExiste = await _igrejaRepository.GetById(entityDTO.IgrejaId);
                    if (igrejaExiste is null)
                    {
                        errors.Add("A Igreja informada não está cadastrada no banco de dados.");
                    }
                    else
                    {
                        // Validação 1:1 - Garante que não haja tentativa de criar outra configuração para a mesma igreja
                        var configExiste = await _configuracoesRepository.GetById(entityDTO.IgrejaId);
                        if (configExiste != null)
                        {
                            errors.Add("Esta Igreja já possui configurações cadastradas. Realize uma atualização (Update) em vez de uma criação.");
                        }
                    }
                }

                // Validação do campo JSON
                if (string.IsNullOrWhiteSpace(entityDTO.ConfiguracaoJson))
                {
                    errors.Add("O campo 'ConfiguracaoJson' é obrigatório.");
                }
                else if (!IsValidJson(entityDTO.ConfiguracaoJson))
                {
                    errors.Add("O formato do 'ConfiguracaoJson' é inválido. Certifique-se de enviar uma estrutura JSON válida.");
                }
            }

            if (errors.Any())
            {
                throw new ValidationException(errors);
            }

            var entity = _mapper.Map<Configuracoes>(entityDTO);
            await _configuracoesRepository.Add(entity);
        }

        // Método auxiliar para validar se a string é um JSON válido
        private bool IsValidJson(string json)
        {
            try
            {
                using (JsonDocument.Parse(json))
                {
                    return true;
                }
            }
            catch (JsonException)
            {
                return false;
            }
        }
    }
}