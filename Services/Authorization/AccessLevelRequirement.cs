using Microsoft.AspNetCore.Authorization;

namespace f_backend_gestafe.Services.Authorization;

public sealed class AccessLevelRequirement : IAuthorizationRequirement
{
    public AccessLevelRequirement(int maximumAccessLevel)
    {
        MaximumAccessLevel = maximumAccessLevel;
    }

    public int MaximumAccessLevel { get; }
}
