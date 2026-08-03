using f_backend_gestafe.Objects.Dtos.Entities;

namespace f_backend_gestafe.Services.Interfaces
{
    public interface IWhatsAppIntegrationService
    {
        Task<bool> SendMessageAsync(SendWhatsAppMessageRequestDTO dto, CancellationToken cancellationToken = default);
    }
}
