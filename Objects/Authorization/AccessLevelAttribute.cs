using f_backend_gestafe.Objects.Enums;
using Microsoft.AspNetCore.Authorization;

namespace f_backend_gestafe.Objects.Authorization;

/// <summary>
/// Restricts an endpoint to user types with an access level equal to or more
/// privileged than the supplied level. Lower numeric levels are more privileged.
/// </summary>
public sealed class AccessLevelAttribute : AuthorizeAttribute
{
    public const string PolicyPrefix = "AccessLevel";

    public AccessLevelAttribute(NivelAcesso nivelAcesso)
    {
        Policy = $"{PolicyPrefix}:{(int)nivelAcesso}";
    }
}
