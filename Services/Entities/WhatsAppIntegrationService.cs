using f_backend_gestafe.Configurations;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace f_backend_gestafe.Services.Entities
{
    public class WhatsAppIntegrationService : IWhatsAppIntegrationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WhatsAppIntegrationService> _logger;

        // Configuração do serializador para omitir campos nulos do JSON enviado
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public WhatsAppIntegrationService(
            HttpClient httpClient,
            IOptions<WhatsAppSettings> settings,
            ILogger<WhatsAppIntegrationService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            var config = settings.Value;
            _httpClient.BaseAddress = new Uri(config.BaseUrl);
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", config.SecretKey);
        }

        public async Task<bool> SendMessageAsync(SendWhatsAppMessageRequestDTO dto, CancellationToken cancellationToken = default)
        {
            try
            {
                // Usamos os _jsonOptions configurados para ignorar campos nulos
                var response = await _httpClient.PostAsJsonAsync("/whatsapp", dto, _jsonOptions, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Mensagem de WhatsApp enfileirada com sucesso para {Phone}", dto.Phone);
                    return true;
                }

                var errorBody = await response.Content.ReadFromJsonAsync<WhatsAppResponseDto>(cancellationToken: cancellationToken);
                var mensagemErro = errorBody?.GetFormattedMessage() ?? "Erro ao desserializar resposta.";

                _logger.LogError("Falha ao solicitar envio de WhatsApp para {Phone}. Status HTTP: {Status}, Detalhe: {Erro}",
                    dto.Phone, response.StatusCode, mensagemErro);

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exceção inesperada ao tentar se comunicar com o microsserviço de WhatsApp.");
                throw;
            }
        }
    }
}
