using System;

namespace TransportLink.Api.Contracts.Auth;

public sealed record RegisterDriverRequest(Guid DriverId, string Email, string Password);
