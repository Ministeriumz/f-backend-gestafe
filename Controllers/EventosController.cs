using f_backend_gestafe.Objects.Contracts;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;

namespace f_backend_gestafe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventosController : Controller
    { 
        private readonly IEventosService _EventosService;
        private readonly Response _response;

        public EventosController(IEventosService EventosService)
        {
            _EventosService = EventosService;
            _response = new Response();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _EventosService.GetAll();
            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = res;
            _response.Message = "Eventos listados com sucesso";

            return Ok(_response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var EventosDTO = await _EventosService.GetById(id);

            if (EventosDTO is null)
            {
                _response.Code = ResponseEnum.NOT_FOUND;
                _response.Data = EventosDTO;
                _response.Message = "Eventos não sucesso";

                return NotFound(_response);
            }

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = EventosDTO;
            _response.Message = "Eventos listados com sucesso";

            return Ok(_response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventosDTO EventosDTO)
        {
            if (EventosDTO is null)
            {
                _response.Code = ResponseEnum.INVALID;
                _response.Data = null;
                _response.Message = "Dados inválidos";

                return BadRequest(_response);
            }

            EventosDTO.Id = 0;
            await _EventosService.Create(EventosDTO);

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = EventosDTO;
            _response.Message = "Evento cadastrado com sucesso";

            return Ok(_response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(EventosDTO EventosDTO)
        {
            var id = EventosDTO.Id;
            if (EventosDTO is null)
            {
                _response.Code = ResponseEnum.INVALID;
                _response.Data = null;
                _response.Message = "Dados inválidos";

                return BadRequest(_response);
            }

            var existingAlunoDTO = await _EventosService.GetById(id);
            if (existingAlunoDTO is null)
            {
                _response.Code = ResponseEnum.NOT_FOUND;
                _response.Data = null;
                _response.Message = "O Evento informado não existe";
                return NotFound(_response);
            }

            await _EventosService.Update(EventosDTO, id);

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = EventosDTO;
            _response.Message = "Eventos atualizada com sucesso";

            return Ok(_response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingAlunoDTO = await _EventosService.GetById(id);
            if (existingAlunoDTO is null)
            {
                _response.Code = ResponseEnum.NOT_FOUND;
                _response.Data = null;
                _response.Message = "O Evento informada não existe";
                return NotFound(_response);
            }

            await _EventosService.Remove(id);

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = null;
            _response.Message = "Evento removida com sucesso";

            return Ok(_response);
        }
    }
}
