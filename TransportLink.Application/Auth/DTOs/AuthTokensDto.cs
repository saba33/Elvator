using System;

namespace TransportLink.Application.Auth.DTOs;

public sealed record AuthTokensDto(
    Guid UserId,
    string Email,
    string Role,
    string AccessToken,
    DateTime AccessTokenExpiresAt,
    string RefreshToken,
    DateTime RefreshTokenExpiresAt,
    Guid? CompanyId,
    Guid? DriverId);
