using f_backend_gestafe.Objects.Contracts;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace f_backend_gestafe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CargoUsuarioController : Controller
    {
        private readonly ICargosUsuarioService _cargosUsuarioService;
        private readonly Response _response;

        public CargoUsuarioController(ICargosUsuarioService cargosUsuarioService)
        {
            _cargosUsuarioService = cargosUsuarioService;
            _response = new Response();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _cargosUsuarioService.GetAll();

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = res;
            _response.Message = "Relações cargo-usuário listadas com sucesso";

            return Ok(_response);
        }

        [HttpGet("{usuarioId}/{cargoId}")]
        public async Task<IActionResult> GetById(int usuarioId, int cargoId)
        {
            var cargoUsuario = await _cargosUsuarioService.GetByCompositeId(usuarioId, cargoId);

            if (cargoUsuario is null)
            {
                _response.Code = ResponseEnum.NOT_FOUND;
                _response.Message = "Relação não encontrada";
                return NotFound(_response);
            }

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = cargoUsuario;
            _response.Message = "Relação encontrada com sucesso";

            return Ok(_response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CargosUsuarioDTO cargosUsuarioDTO)
        {
            await _cargosUsuarioService.Create(cargosUsuarioDTO);

            _response.Code = ResponseEnum.SUCCESS;
            _response.Data = cargosUsuarioDTO;
            _response.Message = "Cargo vinculado ao usuário com sucesso";

            return Ok(_response);
        }

        [HttpDelete("{usuarioId}/{cargoId}")]
        public async Task<IActionResult> Delete(int usuarioId, int cargoId)
        {
            var existing = await _cargosUsuarioService.GetByCompositeId(usuarioId, cargoId);

            if (existing is null)
            {
                _response.Code = ResponseEnum.NOT_FOUND;
                _response.Message = "Relação não encontrada";
                return NotFound(_response);
            }

            await _cargosUsuarioService.RemoveComposite(usuarioId, cargoId);

            _response.Code = ResponseEnum.SUCCESS;
            _response.Message = "Cargo removido do usuário com sucesso";

            return Ok(_response);
        }
    }
}