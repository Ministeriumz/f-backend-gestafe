using AutoMapper;
using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Middleware.Exceptions;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Objects.Models;
using f_backend_gestafe.Services.Interfaces;
using System.Text.Json;

namespace f_backend_gestafe.Services.Entities
{
    public class EscalaService : GenericService<Escala, EscalaDTO>, IEscalaService
    {
        private readonly IEscalaRepository _escalaRepository;
        private readonly ICargosRepository _cargosRepository;
        private readonly IIgrejaRepository _igrejaRepository;
        private readonly ICargosUsuarioRepository _cargosUsuarioRepository;
        private readonly IMapper _mapper;

        public EscalaService(
            IEscalaRepository escalaRepository,
            ICargosRepository cargosRepository,
            IIgrejaRepository igrejaRepository,
            ICargosUsuarioRepository cargosUsuarioRepository,
            IMapper mapper) : base(escalaRepository, mapper)
        {
            _escalaRepository = escalaRepository;
            _cargosRepository = cargosRepository;
            _igrejaRepository = igrejaRepository;
            _cargosUsuarioRepository = cargosUsuarioRepository;
            _mapper = mapper;
        }

        public override async Task Create(EscalaDTO entityDTO)
        {
            var errors = new List<string>();

            if (entityDTO is null)
            {
                errors.Add("O objeto não pode ser nulo.");
            }
            else
            {
                // Validação do campo Data
                if (entityDTO.DataSalvamento == default)
                {
                    errors.Add("O campo 'DataSalvamento' é obrigatório e deve ser uma data válida.");
                }

                // Validação de coerência das Horas
                if (entityDTO.HoraSalvamento < TimeOnly.FromDateTime(DateTime.Today))
                {
                    // Dependendo do seu sistema, 00:00 pode ser válido. Se for, remova esta checagem específica de default.
                    errors.Add("O campo 'HoraSalvamento' deve conter uma data válida.");
                }

                // Validação do campo JSON
                if (string.IsNullOrWhiteSpace(entityDTO.EscalaJson))
                {
                    errors.Add("O campo 'EscalaJson' é obrigatório.");
                }
                else if (!IsValidJson(entityDTO.EscalaJson))
                {
                    errors.Add("O formato do 'EscalaJson' é inválido. Certifique-se de enviar uma estrutura JSON válida.");
                }

                // Validação e verificação de existência do Cargo
                if (entityDTO.IgrejaId <= 0)
                {
                    errors.Add("O campo 'igrejaid' é obrigatório e deve ser um ID válido maior que zero.");
                }
                else
                {
                    var igrejaExiste = await _igrejaRepository.GetById(entityDTO.IgrejaId);
                    if (igrejaExiste is null)
                    {
                        errors.Add("A igreja informada não está cadastrado no banco de dados.");
                    }
                }
            }

            if (errors.Any())
            {
                throw new ValidationException(errors);
            }

            var entity = _mapper.Map<Escala>(entityDTO);
            await _escalaRepository.Add(entity);
        }

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

        public async Task<List<ResponseEscalaDiaDTO>> GerarEscalaAleatoriaAsync(RequestEscalaDTO request)
        {
            // 1. Prevenção de Request nulo
            if (request == null)
            {
                throw new ArgumentException("Os parâmetros da requisição não foram enviados.");
            }

            if (request.DataInicio > request.DataFim)
            {
                throw new ArgumentException("A data de início não pode ser maior que a data de fim.");
            }

            // Prevenção de lista nula (se vier nula, cria uma lista vazia)
            request.DiasDaSemana ??= new List<DayOfWeek>();

            // 2. Busca os dados via Repository
            var cargosUsuarios = await _cargosUsuarioRepository.ObterTodosComRelacionamentosAsync();

            if (cargosUsuarios == null || !cargosUsuarios.Any())
            {
                throw new InvalidOperationException("Nenhum usuário atrelado a cargos foi encontrado.");
            }

            // 3. Agrupamento (Filtramos os nulos AQUI para evitar quebra com dados sujos do banco)
            var usuariosPorCargo = cargosUsuarios
                .Where(cu => cu.Cargo != null && cu.Usuario != null) // <-- PROTEÇÃO CRÍTICA
                .GroupBy(cu => cu.Cargo)
                .ToDictionary(g => g.Key, g => g.Select(cu => cu.Usuario).ToList());

            if (!usuariosPorCargo.Any())
            {
                throw new InvalidOperationException("Relacionamentos encontrados, mas os dados de Cargo ou Usuário estão em branco no banco.");
            }

            // 4. Lógica de filtragem de dias
            var diasValidos = new List<DateTime>();
            for (var data = request.DataInicio.Date; data <= request.DataFim.Date; data = data.AddDays(1))
            {
                if (!request.DiasDaSemana.Any() || request.DiasDaSemana.Contains(data.DayOfWeek))
                {
                    diasValidos.Add(data);
                }
            }

            // NOVA LÓGICA: Ordenar os cargos por escassez.
            // Cargos com MENOS pessoas na lista vêm primeiro (ex: Mesa de Som = 2 pessoas).
            // Cargos com MAIS pessoas vão para o final (ex: Recepção = 15 pessoas).
            var cargosOrdenadosPorPrioridade = usuariosPorCargo
                .OrderBy(c => c.Value.Count)
                .ToList();

            // 5. Lógica de sorteio
            var escalaFinal = new List<ResponseEscalaDiaDTO>();
            var random = new Random();

            foreach (var data in diasValidos)
            {
                var escalaDoDia = new ResponseEscalaDiaDTO
                {
                    Data = data,
                    DiaDaSemana = data.ToString("dddd"),
                    Alocacoes = new List<AlocacaoDTO>()
                };

                var usuariosOcupadosHoje = new HashSet<string>();

                // Usamos a lista ORDENADA POR PRIORIDADE em vez do dicionário comum
                foreach (var cargoKvp in cargosOrdenadosPorPrioridade)
                {
                    var cargo = cargoKvp.Key;
                    var listaUsuariosDoCargo = cargoKvp.Value;

                    // Pega todo mundo deste cargo que AINDA NÃO FOI escalado hoje
                    var usuariosDisponiveis = listaUsuariosDoCargo
                        .Where(u => u != null && !usuariosOcupadosHoje.Contains(u.Nome))
                        .ToList();

                    if (usuariosDisponiveis.Any())
                    {
                        // Sorteia um dos disponíveis
                        int indexSorteado = random.Next(usuariosDisponiveis.Count);
                        var usuarioSorteado = usuariosDisponiveis[indexSorteado];

                        escalaDoDia.Alocacoes.Add(new AlocacaoDTO
                        {
                            Cargo = cargo?.Nome ?? "Cargo Desconhecido",
                            Usuario = usuarioSorteado?.Nome ?? "Usuário Desconhecido"
                        });

                        // Trava a pessoa para não ser usada nos próximos cargos do dia
                        if (usuarioSorteado?.Nome != null)
                        {
                            usuariosOcupadosHoje.Add(usuarioSorteado.Nome);
                        }
                    }
                    else
                    {
                        escalaDoDia.Alocacoes.Add(new AlocacaoDTO
                        {
                            Cargo = cargo?.Nome ?? "Cargo Desconhecido",
                            Usuario = "Sem voluntários disponíveis"
                        });
                    }
                }

                escalaFinal.Add(escalaDoDia);
            }

            return escalaFinal;
        }
    }
}
