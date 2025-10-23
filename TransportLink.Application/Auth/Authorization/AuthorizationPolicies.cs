namespace TransportLink.Application.Auth.Authorization;

public static class AuthorizationPolicies
{
    public const string AdminOnly = nameof(AdminOnly);
    public const string CompanyOnly = nameof(CompanyOnly);
    public const string DriverOnly = nameof(DriverOnly);
}
