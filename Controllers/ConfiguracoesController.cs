using f_backend_gestafe.Objects.Contracts;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace f_backend_gestafe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfiguracoesController : Controller
    {
        private readonly IConfiguracoesService _configuracoesService;
        private readonly Response _response;

        public ConfiguracoesController(IConfiguracoesService configuracoesService)
        {
            _configuracoesService = configuracoesService;
            _response = new Response();
        }

        // ⚠️ Avalie se GetAll realmente faz sentido para 1:1
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _configuracoesService.GetAll();

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = res;
            _response.Message = "Configurações listadas com sucesso";

            return Ok(_response);
        }

        [HttpGet("{igrejaId}")]
        public async Task<IActionResult> GetByIgrejaId(int igrejaId)
        {
            var configuracaoDTO = await _configuracoesService.GetById(igrejaId);

            if (configuracaoDTO is null)
            {
                _response.Code = ResponseEnum.NOT_FOUND;
                _response.Data = null;
                _response.Message = "Configuração não encontrada";

                return NotFound(_response);
            }

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = configuracaoDTO;
            _response.Message = "Configuração listada com sucesso";

            return Ok(_response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ConfiguracoesDTO configuracoesDTO)
        {
            if (configuracoesDTO is null)
            {
                _response.Code = ResponseEnum.INVALID;
                _response.Data = null;
                _response.Message = "Dados inválidos";

                return BadRequest(_response);
            }

            // Aqui você NÃO zera Id
            // porque a PK é IgrejaId e deve vir corretamente preenchido

            await _configuracoesService.Create(configuracoesDTO);

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = configuracoesDTO;
            _response.Message = "Configuração cadastrada com sucesso";

            return Ok(_response);
        }

        [HttpPut("{igrejaId}")]
        public async Task<IActionResult> Put(int igrejaId, ConfiguracoesDTO configuracoesDTO)
        {
            if (configuracoesDTO is null)
            {
                _response.Code = ResponseEnum.INVALID;
                _response.Data = null;
                _response.Message = "Dados inválidos";

                return BadRequest(_response);
            }

            var existingConfiguracao = await _configuracoesService.GetById(igrejaId);

            if (existingConfiguracao is null)
            {
                _response.Code = ResponseEnum.NOT_FOUND;
                _response.Data = null;
                _response.Message = "A Configuração informada não existe";

                return NotFound(_response);
            }

            await _configuracoesService.Update(configuracoesDTO, igrejaId);

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = configuracoesDTO;
            _response.Message = "Configuração atualizada com sucesso";

            return Ok(_response);
        }

        [HttpDelete("{igrejaId}")]
        public async Task<IActionResult> Delete(int igrejaId)
        {
            var existingConfiguracao = await _configuracoesService.GetById(igrejaId);

            if (existingConfiguracao is null)
            {
                _response.Code = ResponseEnum.NOT_FOUND;
                _response.Data = null;
                _response.Message = "A Configuração informada não existe";

                return NotFound(_response);
            }

            await _configuracoesService.Remove(igrejaId);

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = null;
            _response.Message = "Configuração removida com sucesso";

            return Ok(_response);
        }
    }
}