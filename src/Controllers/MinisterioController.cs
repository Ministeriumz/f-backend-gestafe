using f_backend_gestafe.Objects.Contracts;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace f_backend_gestafe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MinisterioController : Controller
    {
        private readonly IMinisterioService _ministerioService;
        private readonly Response _response;

        public MinisterioController(IMinisterioService ministerioService)
        {
            _ministerioService = ministerioService;
            _response = new Response();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var res = await _ministerioService.GetAll();
                _response.Code = ResponseEnum.SUCCESS;
                _response.Data = res;
                _response.Message = "Ministerios listados com sucesso";

                return Ok(_response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar todos os Ministerios! ${ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contentDTO = await _ministerioService.GetById(id);

            if (contentDTO is null)
            {
                _response.Code = ResponseEnum.NOT_FOUND;
                _response.Data = contentDTO;
                _response.Message = "Ministerios não sucesso";

                return NotFound(_response);
            }

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = contentDTO;
            _response.Message = "Ministerio listado com sucesso";

            return Ok(_response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(MinisterioDTO contentDTO)
        {
            if (contentDTO is null)
            {
                _response.Code = ResponseEnum.INVALID;
                _response.Data = null;
                _response.Message = "Dados inválidos";

                return BadRequest(_response);
            }

            try
            {
                contentDTO.Id = 0;
                await _ministerioService.Create(contentDTO);

                _response.Code = ResponseEnum.SUCCESS;
                _response.Data = contentDTO;
                _response.Message = "Ministerios cadastrado com sucesso";

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.Code = ResponseEnum.ERROR;
                _response.Message = "Não foi possível cadastrar o Ministerios";
                _response.Data = new
                {
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace ?? "No stack trace available"
                };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(MinisterioDTO contentDTO)
        {
            var id = contentDTO.Id;
            if (contentDTO is null)
            {
                _response.Code = ResponseEnum.INVALID;
                _response.Data = null;
                _response.Message = "Dados inválidos";

                return BadRequest(_response);
            }

            try
            {
                var existingAlunoDTO = await _ministerioService.GetById(id);
                if (existingAlunoDTO is null)
                {
                    _response.Code = ResponseEnum.NOT_FOUND;
                    _response.Data = null;
                    _response.Message = "O Ministerio informado não existe";
                    return NotFound(_response);
                }

                await _ministerioService.Update(contentDTO, id);

                _response.Code = ResponseEnum.SUCCESS;
                _response.Data = contentDTO;
                _response.Message = "Ministerio atualizado com sucesso";

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.Code = ResponseEnum.ERROR;
                _response.Message = "Ocorreu um erro ao tentar atualizar os dados do Ministerio";
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
                var existingAlunoDTO = await _ministerioService.GetById(id);
                if (existingAlunoDTO is null)
                {
                    _response.Code = ResponseEnum.NOT_FOUND;
                    _response.Data = null;
                    _response.Message = "O Ministerio informado não existe";
                    return NotFound(_response);
                }

                await _ministerioService.Remove(id);

                _response.Code = ResponseEnum.SUCCESS;
                _response.Data = null;
                _response.Message = "Ministerio removido com sucesso";

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.Code = ResponseEnum.ERROR;
                _response.Message = "Ocorreu um erro ao tentar remover o Ministerio";
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
