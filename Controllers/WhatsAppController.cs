using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace f_backend_gestafe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WhatsAppController : ControllerBase
    {
        private readonly IWhatsAppIntegrationService _whatsAppService;

        public WhatsAppController(IWhatsAppIntegrationService whatsAppService)
        {
            _whatsAppService = whatsAppService;
        }

        [HttpPost("send-test")]
        public async Task<IActionResult> SendNotification([FromBody] SendWhatsAppMessageRequestDTO dto)
        {
            var success = await _whatsAppService.SendMessageAsync(dto);

            if (success)
                return Ok(new { status = "Mensagem aceita para envio!" });

            return BadRequest(new { status = "Não foi possível registrar o envio." });
        }
    }
}
