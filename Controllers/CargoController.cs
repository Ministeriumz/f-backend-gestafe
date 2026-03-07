using f_backend_gestafe.Objects.Contracts;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Services.Entities;
using f_backend_gestafe.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace f_backend_gestafe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CargoController : Controller
    {
        private readonly ICargosService _cargosService;
        private readonly Response _response;

        public CargoController(ICargosService cargosService)
        {
            _cargosService = cargosService;
            _response = new Response();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _cargosService.GetAll();

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = res;
            _response.Message = "Cargos listados com sucesso";

            return Ok(_response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cargoDTO = await _cargosService.GetById(id);

            if (cargoDTO is null)
            {
                _response.Code = ResponseEnum.NOT_FOUND;
                _response.Data = null;
                _response.Message = "Cargo não encontrado";

                return NotFound(_response);
            }

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = cargoDTO;
            _response.Message = "Cargo encontrado com sucesso";

            return Ok(_response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CargosDTO cargosDTO)
        {
            if (cargosDTO is null)
            {
                _response.Code = ResponseEnum.INVALID;
                _response.Data = null;
                _response.Message = "Dados inválidos";

                return BadRequest(_response);
            }

            await _cargosService.Create(cargosDTO);

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = cargosDTO;
            _response.Message = "Cargo cadastrado com sucesso";

            return Ok(_response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, CargosDTO cargosDTO)
        {
            if (cargosDTO is null)
            {
                _response.Code = ResponseEnum.INVALID;
                _response.Data = null;
                _response.Message = "Dados inválidos";

                return BadRequest(_response);
            }

            var existingCargo = await _cargosService.GetById(id);

            if (existingCargo is null)
            {
                _response.Code = ResponseEnum.NOT_FOUND;
                _response.Data = null;
                _response.Message = "Cargo não encontrado";

                return NotFound(_response);
            }

            await _cargosService.Update(cargosDTO, id);

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = cargosDTO;
            _response.Message = "Cargo atualizado com sucesso";

            return Ok(_response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingCargo = await _cargosService.GetById(id);

            if (existingCargo is null)
            {
                _response.Code = ResponseEnum.NOT_FOUND;
                _response.Data = null;
                _response.Message = "Cargo não encontrado";

                return NotFound(_response);
            }

            await _cargosService.Remove(id);

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = null;
            _response.Message = "Cargo removido com sucesso";

            return Ok(_response);
        }
    }
}