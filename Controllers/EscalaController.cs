using f_backend_gestafe.Objects.Contracts;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace f_backend_gestafe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EscalaController : Controller
    {
        private readonly IEscalaService _escalaService;
        private readonly Response _response;

        public EscalaController(IEscalaService escalaService)
        {
            _escalaService = escalaService;
            _response = new Response();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _escalaService.GetAll();

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = res;
            _response.Message = "Escalas listadas com sucesso";

            return Ok(_response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var escalaDTO = await _escalaService.GetById(id);

            if (escalaDTO is null)
            {
                _response.Code = ResponseEnum.NOT_FOUND;
                _response.Data = null;
                _response.Message = "Escala não encontrada";

                return NotFound(_response);
            }

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = escalaDTO;
            _response.Message = "Escala encontrada com sucesso";

            return Ok(_response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(EscalaDTO escalaDTO)
        {
            if (escalaDTO is null)
            {
                _response.Code = ResponseEnum.INVALID;
                _response.Data = null;
                _response.Message = "Dados inválidos";

                return BadRequest(_response);
            }

            await _escalaService.Create(escalaDTO);

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = escalaDTO;
            _response.Message = "Escala cadastrada com sucesso";

            return Ok(_response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EscalaDTO escalaDTO)
        {
            if (escalaDTO is null)
            {
                _response.Code = ResponseEnum.INVALID;
                _response.Data = null;
                _response.Message = "Dados inválidos";

                return BadRequest(_response);
            }

            var existingEscala = await _escalaService.GetById(id);

            if (existingEscala is null)
            {
                _response.Code = ResponseEnum.NOT_FOUND;
                _response.Data = null;
                _response.Message = "Escala não encontrada";

                return NotFound(_response);
            }

            await _escalaService.Update(escalaDTO, id);

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = escalaDTO;
            _response.Message = "Escala atualizada com sucesso";

            return Ok(_response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingEscala = await _escalaService.GetById(id);

            if (existingEscala is null)
            {
                _response.Code = ResponseEnum.NOT_FOUND;
                _response.Data = null;
                _response.Message = "Escala não encontrada";

                return NotFound(_response);
            }

            await _escalaService.Remove(id);

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = null;
            _response.Message = "Escala removida com sucesso";

            return Ok(_response);
        }
    }
}