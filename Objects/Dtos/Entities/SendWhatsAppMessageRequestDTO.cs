using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace f_backend_gestafe.Objects.Dtos.Entities
{
    public record SendWhatsAppMessageRequestDTO
    {
        [JsonPropertyName("phone")]
        [Required]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "O telefone deve ter entre 10 e 15 caracteres.")]
        public string Phone { get; init; } = string.Empty;

        [JsonPropertyName("text")]
        [Required]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "A mensagem deve ter no máximo 500 caracteres.")]
        public string Text { get; init; } = string.Empty;

        [JsonPropertyName("webhook")]
        [Url(ErrorMessage = "O formato da URL do webhook é inválido.")]
        public string? Webhook { get; init; }

        [JsonPropertyName("forAt")]
        public DateTime? ForAt { get; init; }
    }

    public record WhatsAppResponseDto
    {
        [JsonPropertyName("message")]
        public JsonElement? Message { get; init; }

        [JsonPropertyName("error")]
        public string? Error { get; init; }

        /// <summary>
        /// Método utilitário para extrair a mensagem de erro formatada independente da estrutura.
        /// </summary>
        public string GetFormattedMessage()
        {
            if (Message.HasValue)
            {
                if (Message.Value.ValueKind == JsonValueKind.String)
                {
                    return Message.Value.GetString() ?? string.Empty;
                }

                // Se for o objeto de erro do Zod (Bad Request 400)
                return Message.Value.GetRawText();
            }

            return Error ?? "Erro desconhecido retornado pelo microsserviço.";
        }
    }
}
