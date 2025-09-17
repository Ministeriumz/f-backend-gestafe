using f_backend_gestafe.Objects.Contracts;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace f_backend_gestafe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TipoUsuarioController : Controller
    {
        private readonly ITipoUsuarioService _tipoUsuarioService;
        private readonly Response _response;

        public TipoUsuarioController(ITipoUsuarioService tipoUsuarioService)
        {
            _tipoUsuarioService = tipoUsuarioService;
            _response = new Response();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var res = await _tipoUsuarioService.GetAll();
                _response.Code = ResponseEnum.SUCCESS;
                _response.Data = res;
                _response.Message = "Tipos de usuário listados com sucesso";

                return Ok(_response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar todos os tipos de usuário! {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tipoUsuarioDTO = await _tipoUsuarioService.GetById(id);

            if (tipoUsuarioDTO is null)
            {
                _response.Code = ResponseEnum.NOT_FOUND;
                _response.Data = tipoUsuarioDTO;
                _response.Message = "Tipo de usuário não encontrado";

                return NotFound(_response);
            }

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = tipoUsuarioDTO;
            _response.Message = "Tipo de usuário listado com sucesso";

            return Ok(_response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(TipoUsuarioDTO tipoUsuarioDTO)
        {
            if (tipoUsuarioDTO is null)
            {
                _response.Code = ResponseEnum.INVALID;
                _response.Data = null;
                _response.Message = "Dados inválidos";

                return BadRequest(_response);
            }

            try
            {
                tipoUsuarioDTO.Id = 0;
                await _tipoUsuarioService.Create(tipoUsuarioDTO);

                _response.Code = ResponseEnum.SUCCESS;
                _response.Data = tipoUsuarioDTO;
                _response.Message = "Tipo de usuário cadastrado com sucesso";

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.Code = ResponseEnum.ERROR;
                _response.Message = "Não foi possível cadastrar o tipo de usuário";
                _response.Data = new
                {
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace ?? "No stack trace available"
                };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpPatch("AtualizarTipoUsuarioPorId{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] TipoUsuarioPatchDTO tipoUsuarioDTO)
        {
            if (tipoUsuarioDTO is null)
            {
                _response.Code = ResponseEnum.INVALID;
                _response.Data = null;
                _response.Message = "Dados inválidos";
                return BadRequest(_response);
            }

            try
            {
                var existingTipoUsuario = await _tipoUsuarioService.GetById(id);
                if (existingTipoUsuario is null)
                {
                    _response.Code = ResponseEnum.NOT_FOUND;
                    _response.Data = null;
                    _response.Message = "Tipo de Usuário não encontrado";
                    return NotFound(_response);
                }

                // Atualiza apenas os campos que vierem
                existingTipoUsuario.Nome = tipoUsuarioDTO.Nome ?? existingTipoUsuario.Nome;

                await _tipoUsuarioService.Update(existingTipoUsuario, id);

                _response.Code = ResponseEnum.SUCCESS;
                _response.Data = existingTipoUsuario;
                _response.Message = "Tipo de Usuário atualizado parcialmente com sucesso";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.Code = ResponseEnum.ERROR;
                _response.Message = "Erro ao atualizar o tipo de usuário";
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
        public async Task<IActionResult> Put(TipoUsuarioDTO tipoUsuarioDTO)
        {
            var id = tipoUsuarioDTO.Id;
            if (tipoUsuarioDTO is null)
            {
                _response.Code = ResponseEnum.INVALID;
                _response.Data = null;
                _response.Message = "Dados inválidos";

                return BadRequest(_response);
            }

            try
            {
                var existingTipoUsuarioDTO = await _tipoUsuarioService.GetById(id);
                if (existingTipoUsuarioDTO is null)
                {
                    _response.Code = ResponseEnum.NOT_FOUND;
                    _response.Data = null;
                    _response.Message = "O tipo de usuário informado não existe";
                    return NotFound(_response);
                }

                await _tipoUsuarioService.Update(tipoUsuarioDTO, id);

                _response.Code = ResponseEnum.SUCCESS;
                _response.Data = tipoUsuarioDTO;
                _response.Message = "Tipo de usuário atualizado com sucesso";

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.Code = ResponseEnum.ERROR;
                _response.Message = "Ocorreu um erro ao tentar atualizar os dados do tipo de usuário";
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
                var existingTipoUsuarioDTO = await _tipoUsuarioService.GetById(id);
                if (existingTipoUsuarioDTO is null)
                {
                    _response.Code = ResponseEnum.NOT_FOUND;
                    _response.Data = null;
                    _response.Message = "O tipo de usuário informado não existe";
                    return NotFound(_response);
                }

                await _tipoUsuarioService.Remove(id);

                _response.Code = ResponseEnum.SUCCESS;
                _response.Data = null;
                _response.Message = "Tipo de usuário removido com sucesso";

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.Code = ResponseEnum.ERROR;
                _response.Message = "Ocorreu um erro ao tentar remover o tipo de usuário";
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
