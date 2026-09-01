using f_backend_gestafe.Objects.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace f_backend_gestafe.Services.Authorization;

/// <summary>
/// Creates access-level policies on demand, so adding a new level only requires
/// adding it to <see cref="Objects.Enums.NivelAcesso"/> and applying the attribute.
/// </summary>
public sealed class AccessLevelPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public AccessLevelPolicyProvider(IOptions<AuthorizationOptions> options)
        : base(options)
    {
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var prefix = $"{AccessLevelAttribute.PolicyPrefix}:";

        if (policyName.StartsWith(prefix, StringComparison.Ordinal)
            && int.TryParse(policyName[prefix.Length..], out var maximumAccessLevel)
            && maximumAccessLevel >= 0)
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddRequirements(new AccessLevelRequirement(maximumAccessLevel))
                .Build();
        }

        return await base.GetPolicyAsync(policyName);
    }
}
