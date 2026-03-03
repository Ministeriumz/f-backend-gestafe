using f_backend_gestafe.Objects.Contracts;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace f_backend_gestafe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FinanceiroController : ControllerBase
    {
        private readonly IFinanceiroService _financeiroService;
        private readonly Response _response;

        public FinanceiroController(IFinanceiroService financeiroService)
        {
            _financeiroService = financeiroService;
            _response = new Response();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _financeiroService.GetAll();

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = res;
            _response.Message = "Relatórios financeiros listados com sucesso";

            return Ok(_response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var financeiroDTO = await _financeiroService.GetById(id);

            if (financeiroDTO is null)
            {
                _response.Code = ResponseEnum.NOT_FOUND;
                _response.Data = null;
                _response.Message = "Registro financeiro não encontrado";

                return NotFound(_response);
            }

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = financeiroDTO;
            _response.Message = "Registro financeiro listado com sucesso";

            return Ok(_response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FinanceiroDTO financeiroDTO)
        {
            if (financeiroDTO is null)
            {
                _response.Code = ResponseEnum.INVALID;
                _response.Data = null;
                _response.Message = "Dados inválidos";

                return BadRequest(_response);
            }

            financeiroDTO.Id = 0;

            await _financeiroService.Create(financeiroDTO);

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = financeiroDTO;
            _response.Message = "Registro financeiro cadastrado com sucesso";

            return Ok(_response);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] FinanceiroDTO financeiroDTO)
        {
            if (financeiroDTO is null)
            {
                _response.Code = ResponseEnum.INVALID;
                _response.Data = null;
                _response.Message = "Dados inválidos";

                return BadRequest(_response);
            }

            var existingFinanceiroDTO = await _financeiroService.GetById(financeiroDTO.Id);

            if (existingFinanceiroDTO is null)
            {
                _response.Code = ResponseEnum.NOT_FOUND;
                _response.Data = null;
                _response.Message = "Registro financeiro não existe";

                return NotFound(_response);
            }

            await _financeiroService.Update(financeiroDTO, financeiroDTO.Id);

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = financeiroDTO;
            _response.Message = "Registro financeiro atualizado com sucesso";

            return Ok(_response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingFinanceiroDTO = await _financeiroService.GetById(id);

            if (existingFinanceiroDTO is null)
            {
                _response.Code = ResponseEnum.NOT_FOUND;
                _response.Data = null;
                _response.Message = "Registro financeiro não existe";

                return NotFound(_response);
            }

            await _financeiroService.Remove(id);

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = null;
            _response.Message = "Registro financeiro removido com sucesso";

            return Ok(_response);
        }
    }
}