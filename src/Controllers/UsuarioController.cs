using f_backend_gestafe.Objects.Contracts;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Services.Interfaces;
using f_backend_gestafe.src.Objects.Dtos.Entities;
using Microsoft.AspNetCore.Mvc;

namespace f_backend_gestafe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly Response _response;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
            _response = new Response();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var res = await _usuarioService.GetAll();
                _response.Code = ResponseEnum.SUCCESS;
                _response.Data = res;
                _response.Message = "Usuários listados com sucesso";

                return Ok(_response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar todos os usuários! {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuarioDTO = await _usuarioService.GetById(id);

            if (usuarioDTO is null)
            {
                _response.Code = ResponseEnum.NOT_FOUND;
                _response.Data = usuarioDTO;
                _response.Message = "Usuário não encontrado";

                return NotFound(_response);
            }

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = usuarioDTO;
            _response.Message = "Usuário listado com sucesso";

            return Ok(_response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(UsuarioDTO usuarioDTO)
        {
            if (usuarioDTO is null)
            {
                _response.Code = ResponseEnum.INVALID;
                _response.Data = null;
                _response.Message = "Dados inválidos";

                return BadRequest(_response);
            }

            try
            {
                usuarioDTO.Id = 0;
                await _usuarioService.Create(usuarioDTO);

                _response.Code = ResponseEnum.SUCCESS;
                _response.Data = usuarioDTO;
                _response.Message = "Usuário cadastrado com sucesso";

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.Code = ResponseEnum.ERROR;
                _response.Message = "Não foi possível cadastrar o usuário";
                _response.Data = new
                {
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace ?? "No stack trace available"
                };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpPatch("AtualizarUsuarioPorId{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] UsuarioPatchDTO usuarioDTO)
        {
            if (usuarioDTO is null)
            {
                _response.Code = ResponseEnum.INVALID;
                _response.Data = null;
                _response.Message = "Dados inválidos";
                return BadRequest(_response);
            }

            try
            {
                var existingUsuario = await _usuarioService.GetById(id);
                if (existingUsuario is null)
                {
                    _response.Code = ResponseEnum.NOT_FOUND;
                    _response.Data = null;
                    _response.Message = "Usuário não encontrado";
                    return NotFound(_response);
                }

                // Atualiza apenas os campos que vierem
                existingUsuario.Nome = usuarioDTO.Nome ?? existingUsuario.Nome;
                existingUsuario.Sobrenome = usuarioDTO.Sobrenome ?? existingUsuario.Sobrenome;
                existingUsuario.Telefone = usuarioDTO.Telefone ?? existingUsuario.Telefone;
                existingUsuario.Email = usuarioDTO.Email ?? existingUsuario.Email; // novo campo
                existingUsuario.Senha = usuarioDTO.Senha ?? existingUsuario.Senha; // novo campo
                //existingUsuario.IdIgreja = usuarioDTO.IdIgreja ?? existingUsuario.IdIgreja;
                //existingUsuario.IdTipoUsuario = usuarioDTO.IdTipoUsuario ?? existingUsuario.IdTipoUsuario;

                await _usuarioService.Update(existingUsuario, id);

                _response.Code = ResponseEnum.SUCCESS;
                _response.Data = existingUsuario;
                _response.Message = "Usuário atualizado parcialmente com sucesso";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.Code = ResponseEnum.ERROR;
                _response.Message = "Erro ao atualizar o usuário";
                _response.Data = new
                {
                    ErrorMessage = ex.Message,
                    InnerErrorMessage = ex.InnerException?.Message ?? "No inner exception",
                    StackTrace = ex.StackTrace ?? "No stack trace available"
                };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(UsuarioDTO usuarioDTO)
        {
            var id = usuarioDTO.Id;
            if (usuarioDTO is null)
            {
                _response.Code = ResponseEnum.INVALID;
                _response.Data = null;
                _response.Message = "Dados inválidos";

                return BadRequest(_response);
            }

            try
            {
                var existingUsuarioDTO = await _usuarioService.GetById(id);
                if (existingUsuarioDTO is null)
                {
                    _response.Code = ResponseEnum.NOT_FOUND;
                    _response.Data = null;
                    _response.Message = "O usuário informado não existe";
                    return NotFound(_response);
                }

                await _usuarioService.Update(usuarioDTO, id);

                _response.Code = ResponseEnum.SUCCESS;
                _response.Data = usuarioDTO;
                _response.Message = "Usuário atualizado com sucesso";

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.Code = ResponseEnum.ERROR;
                _response.Message = "Ocorreu um erro ao tentar atualizar os dados do usuário";
                _response.Data = new
                {
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace ?? "No stack trace available"
                };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existingUsuarioDTO = await _usuarioService.GetById(id);
                if (existingUsuarioDTO is null)
                {
                    _response.Code = ResponseEnum.NOT_FOUND;
                    _response.Data = null;
                    _response.Message = "O usuário informado não existe";
                    return NotFound(_response);
                }

                await _usuarioService.Remove(id);

                _response.Code = ResponseEnum.SUCCESS;
                _response.Data = null;
                _response.Message = "Usuário removido com sucesso";

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.Code = ResponseEnum.ERROR;
                _response.Message = "Ocorreu um erro ao tentar remover o usuário";
                _response.Data = new
                {
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace ?? "No stack trace available"
                };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }
    }
}