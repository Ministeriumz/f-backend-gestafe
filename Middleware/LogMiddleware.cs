using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Services.Interfaces;
using f_backend_gestafe.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace f_backend_gestafe.Middleware;

public class LogMiddleware
{
    private readonly RequestDelegate _next;

    public LogMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ILogService logService, IHubContext<LogHub> hubContext)
    {
        // Executa o endpoint primeiro
        await _next(context);

        Console.WriteLine("Middleware executou");

        try
        {
            // Evita logar requisições do Swagger
            if (context.Request.Path.StartsWithSegments("/swagger") ||
                context.Request.Path.StartsWithSegments("/auth/login"))
                return;

            int? idUsuario = null;
            var idUsuarioClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(idUsuarioClaim, out var parsedId))
            {
                idUsuario = parsedId;
            }

            var log = new LogDTO
            {
                Data = DateTime.UtcNow.Date,
                Hora = DateTime.UtcNow.TimeOfDay,
                Acao = $"{context.Request.Method} {context.Request.Path}",
                IdUsuario = idUsuario
            };

            await logService.Create(log);
            await hubContext.Clients.All.SendAsync("ReceberLog", log);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro no LogMiddleware: {ex.Message}");
        }
    }
}