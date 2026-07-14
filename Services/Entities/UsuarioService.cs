using AutoMapper;
using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Middleware.Exceptions;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Objects.Models;
using f_backend_gestafe.Services.Interfaces;
using f_backend_gestafe.Services.Security;
using System.Text.RegularExpressions;

namespace f_backend_gestafe.Services.Entities
{
    public class UsuarioService : GenericService<Usuario, UsuarioDTO>, IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IIgrejaRepository _igrejaRepository;
        private readonly ITipoUsuarioRepository _tipoUsuarioRepository;
        private readonly IMapper _mapper;

        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            IIgrejaRepository igrejaRepository,
            ITipoUsuarioRepository tipoUsuarioRepository,
            IMapper mapper) : base(usuarioRepository, mapper)
        {
            _usuarioRepository = usuarioRepository;
            _igrejaRepository = igrejaRepository;
            _tipoUsuarioRepository = tipoUsuarioRepository;
            _mapper = mapper;
        }

        public override async Task Create(UsuarioDTO entityDTO)
        {
            var errors = new List<string>();

            if (entityDTO is null)
            {
                errors.Add("O objeto não pode ser nulo.");
            }
            else
            {
                // Validação do campo Nome
                if (string.IsNullOrWhiteSpace(entityDTO.Nome))
                {
                    errors.Add("O campo 'Nome' é obrigatório.");
                }
                else if (entityDTO.Nome.Length > 100)
                {
                    errors.Add("O campo 'Nome' deve ter no máximo 100 caracteres.");
                }

                // Validação do campo Sobrenome
                if (string.IsNullOrWhiteSpace(entityDTO.Sobrenome))
                {
                    errors.Add("O campo 'Sobrenome' é obrigatório.");
                }
                else if (entityDTO.Sobrenome.Length > 100)
                {
                    errors.Add("O campo 'Sobrenome' deve ter no máximo 100 caracteres.");
                }

                // Validação do campo Telefone
                if (string.IsNullOrWhiteSpace(entityDTO.Telefone))
                {
                    errors.Add("O campo 'Telefone' é obrigatório.");
                }
                else if (entityDTO.Telefone.Length > 18)
                {
                    errors.Add("O campo 'Telefone' deve ter no máximo 18 caracteres.");
                }

                // Validação do campo Email
                if (string.IsNullOrWhiteSpace(entityDTO.Email))
                {
                    errors.Add("O campo 'Email' é obrigatório.");
                }
                else if (entityDTO.Email.Length > 100)
                {
                    errors.Add("O campo 'Email' deve ter no máximo 100 caracteres.");
                }
                else if (!Regex.IsMatch(entityDTO.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    errors.Add("O 'Email' informado não possui um formato válido.");
                }

                // Validação do campo Senha
                if (string.IsNullOrWhiteSpace(entityDTO.Senha))
                {
                    errors.Add("O campo 'Senha' é obrigatório.");
                }
                else if (entityDTO.Senha.Length < 8)
                {
                    errors.Add("O campo 'Senha' deve ter no mínimo 8 caracteres.");
                }
                else if (entityDTO.Senha.Length > 255)
                {
                    errors.Add("O campo 'Senha' deve ter no máximo 255 caracteres.");
                }

                // Validação de existência da Igreja
                if (entityDTO.IdIgreja <= 0)
                {
                    errors.Add("O campo 'IdIgreja' é obrigatório e deve ser um ID válido.");
                }
                else
                {
                    var igrejaExiste = await _igrejaRepository.GetById(entityDTO.IdIgreja);
                    if (igrejaExiste is null)
                    {
                        errors.Add("A Igreja informada não está cadastrada no banco de dados.");
                    }
                }

                // Validação de existência do Tipo de Usuário
                if (entityDTO.IdTipoUsuario <= 0)
                {
                    errors.Add("O campo 'IdTipoUsuario' é obrigatório e deve ser um ID válido.");
                }
                else
                {
                    var tipoUsuarioExiste = await _tipoUsuarioRepository.GetById(entityDTO.IdTipoUsuario);
                    if (tipoUsuarioExiste is null)
                    {
                        errors.Add("O Tipo de Usuário informado não está cadastrado no banco de dados.");
                    }
                }
            }

            if (errors.Any())
            {
                throw new ValidationException(errors);
            }

            var entity = _mapper.Map<Usuario>(entityDTO);

            // Observação: É recomendado aplicar um hash na senha (ex: BCrypt) antes de salvar no banco,
            // que seria feito neste ponto, logo antes de enviar a entidade para o repositório.

            entity.Senha = PasswordHasher.Hash(entity.Senha);

            await _usuarioRepository.Add(entity);
        }
    }
}