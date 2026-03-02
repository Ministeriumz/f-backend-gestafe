using f_backend_gestafe.Objects.Contracts;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Services.Interfaces;
using f_backend_gestafe.Objects.Dtos.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;


namespace f_backend_gestafe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IgrejaController : Controller
    {
        private readonly IIgrejaService _igrejaService;
        private readonly Response _response;

        public IgrejaController(IIgrejaService igrejaService)
        {
            _igrejaService = igrejaService;
            _response = new Response();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _igrejaService.GetAll();
            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = res;
            _response.Message = "Igrejas listadas com sucesso";

            return Ok(_response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var igrejaDTO = await _igrejaService.GetById(id);

            if (igrejaDTO is null)
            {
                _response.Code = ResponseEnum.NOT_FOUND;
                _response.Data = igrejaDTO;
                _response.Message = "Igreja não sucesso";

                return NotFound(_response);
            }

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = igrejaDTO;
            _response.Message = "Aluno listado com sucesso";

            return Ok(_response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(IgrejaDTO igrejaDTO)
        {
            if (igrejaDTO is null)
            {
                _response.Code = ResponseEnum.INVALID;
                _response.Data = null;
                _response.Message = "Dados inválidos";

                return BadRequest(_response);
            }

            igrejaDTO.Id = 0;
            await _igrejaService.Create(igrejaDTO);

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = igrejaDTO;
            _response.Message = "Igreja cadastrada com sucesso";

            return Ok(_response);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] IgrejaPatchDTO igrejaDTO)
        {
            if (igrejaDTO is null)
            {
                _response.Code = ResponseEnum.INVALID;
                _response.Data = null;
                _response.Message = "Dados inválidos";
                return BadRequest(_response);
            }

            var existingIgreja = await _igrejaService.GetById(id);
            if (existingIgreja is null)
            {
                _response.Code = ResponseEnum.NOT_FOUND;
                _response.Data = null;
                _response.Message = "Igreja não encontrada";
                return NotFound(_response);
            }

            // Atualiza apenas os campos que vierem
            existingIgreja.Nome = igrejaDTO.Nome ?? existingIgreja.Nome;

            await _igrejaService.Update(existingIgreja, id);

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = existingIgreja;
            _response.Message = "Igreja atualizada parcialmente com sucesso";
            return Ok(_response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(IgrejaDTO igrejaDTO)
        {
            var id = igrejaDTO.Id;
            if (igrejaDTO is null)
            {
                _response.Code = ResponseEnum.INVALID;
                _response.Data = null;
                _response.Message = "Dados inválidos";

                return BadRequest(_response);
            }

            var existingAlunoDTO = await _igrejaService.GetById(id);
            if (existingAlunoDTO is null)
            {
                _response.Code = ResponseEnum.NOT_FOUND;
                _response.Data = null;
                _response.Message = "A Igreja informada não existe";
                return NotFound(_response);
            }

            await _igrejaService.Update(igrejaDTO, id);

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = igrejaDTO;
            _response.Message = "Igreja atualizada com sucesso";

            return Ok(_response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingAlunoDTO = await _igrejaService.GetById(id);
            if (existingAlunoDTO is null)
            {
                _response.Code = ResponseEnum.NOT_FOUND;
                _response.Data = null;
                _response.Message = "A Igreja informada não existe";
                return NotFound(_response);
            }

            await _igrejaService.Remove(id);

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = null;
            _response.Message = "Igreja removida com sucesso";

            return Ok(_response);
        }
    }
}
