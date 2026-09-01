using f_backend_gestafe.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace f_backend_gestafe.Services.Authorization;

public sealed class AccessLevelAuthorizationHandler
    : AuthorizationHandler<AccessLevelRequirement>
{
    private readonly AppDbContext _context;

    public AccessLevelAuthorizationHandler(AppDbContext context)
    {
        _context = context;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AccessLevelRequirement requirement)
    {
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return;
        }

        // The database remains the authority for access. This prevents an
        // already-issued token from retaining access after its user type or
        // level is changed by a superadministrator.
        var usuario = await _context.Usuario
            .AsNoTracking()
            .Include(u => u.TipoUsuario)
            .SingleOrDefaultAsync(u => u.Id == userId);

        var accessLevel = usuario?.TipoUsuario is null
            ? -1
            : (int)usuario.TipoUsuario.NivelAcesso;

        if (accessLevel >= 0 && accessLevel <= requirement.MaximumAccessLevel)
        {
            context.Succeed(requirement);
        }
    }
}
