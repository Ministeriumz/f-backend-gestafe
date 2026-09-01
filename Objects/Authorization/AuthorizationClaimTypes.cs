namespace f_backend_gestafe.Objects.Authorization;

/// <summary>
/// Claim names emitted in the application JWT. They intentionally describe the
/// authorization model rather than the display name of a user type.
/// </summary>
public static class AuthorizationClaimTypes
{
    public const string UserTypeId = "user_type_id";
    public const string AccessLevel = "access_level";
}
