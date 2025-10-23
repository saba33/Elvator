using System;

namespace TransportLink.Api.Contracts.Auth;

public sealed record RegisterCompanyRequest(Guid CompanyId, string Email, string Password);
