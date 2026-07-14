using AutoMapper;
using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Middleware.Exceptions;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Objects.Models;
using f_backend_gestafe.Services.Interfaces;

namespace f_backend_gestafe.Services.Entities
{
    public class CargosUsuarioService : GenericService<CargosUsuario, CargosUsuarioDTO>, ICargosUsuarioService
    {
        private readonly ICargosUsuarioRepository _cargosUsuarioRepository;
        private readonly ICargosRepository _cargosRepository;
        private readonly IUsuarioRepository _usuariosRepository; // Injetando o repositório de Usuários
        private readonly IMapper _mapper;

        public CargosUsuarioService(
            ICargosUsuarioRepository cargosUsuarioRepository,
            ICargosRepository cargosRepository,
            IUsuarioRepository usuariosRepository,
            IMapper mapper) : base(cargosUsuarioRepository, mapper)
        {
            _cargosUsuarioRepository = cargosUsuarioRepository;
            _cargosRepository = cargosRepository;
            _usuariosRepository = usuariosRepository;
            _mapper = mapper;
        }

        public async Task<CargosUsuarioDTO> GetByCompositeId(int usuarioId, int cargoId)
        {
            var entity = await _cargosUsuarioRepository.GetByCompositeId(usuarioId, cargoId);

            return _mapper.Map<CargosUsuarioDTO>(entity);
        }

        public async Task RemoveComposite(int usuarioId, int cargoId)
        {
            await _cargosUsuarioRepository.RemoveComposite(usuarioId, cargoId);
        }

        public override async Task Create(CargosUsuarioDTO entityDTO)
        {
            var errors = new List<string>();

            if (entityDTO is null)
            {
                errors.Add("O objeto não pode ser nulo.");
            }
            else
            {
                // Validação e verificação de existência do Usuário
                if (entityDTO.IdUsuario <= 0)
                {
                    errors.Add("O campo 'IdUsuario' é obrigatório e deve ser um ID válido.");
                }
                else
                {
                    var usuarioExiste = await _usuariosRepository.GetById(entityDTO.IdUsuario);
                    if (usuarioExiste is null)
                    {
                        errors.Add("O Usuário informado não está cadastrado no banco de dados.");
                    }
                }

                // Validação e verificação de existência do Cargo
                if (entityDTO.IdCargo <= 0)
                {
                    errors.Add("O campo 'IdCargo' é obrigatório e deve ser um ID válido.");
                }
                else
                {
                    var cargoExiste = await _cargosRepository.GetById(entityDTO.IdCargo);
                    if (cargoExiste is null)
                    {
                        errors.Add("O Cargo informado não está cadastrado no banco de dados.");
                    }
                }

                // (Opcional - Recomendado) Verifica se o vínculo já existe para não dar erro de chave duplicada
                if (entityDTO.IdUsuario > 0 && entityDTO.IdCargo > 0 && !errors.Any())
                {
                    var vinculoExistente = await _cargosUsuarioRepository.GetByUsuarioECargoAsync(entityDTO.IdUsuario, entityDTO.IdCargo);
                    if (vinculoExistente != null)
                    {
                        errors.Add("Este usuário já possui o cargo informado cadastrado.");
                    }
                }
            }

            if (errors.Any())
            {
                throw new ValidationException(errors);
            }

            var entity = _mapper.Map<CargosUsuario>(entityDTO);
            await _cargosUsuarioRepository.Add(entity);
        }
    }
}