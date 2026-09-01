using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using f_backend_gestafe.Objects.Authorization;
using f_backend_gestafe.Objects.Enums;

namespace f_backend_gestafe.Hubs
{
    [AccessLevel(NivelAcesso.SuperAdministrador)]
    public class LogHub : Hub
    {

    }
}
