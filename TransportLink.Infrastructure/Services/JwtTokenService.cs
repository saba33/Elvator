using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TransportLink.Application.Auth.Authorization;
using TransportLink.Application.Auth.DTOs;
using TransportLink.Application.Common.Interfaces;
using TransportLink.Domain.Entities;
using TransportLink.Domain.Enums;

namespace TransportLink.Infrastructure.Services;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public AuthTokensDto GenerateTokens(User user)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var issuer = _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT issuer is not configured.");
        var audience = _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT audience is not configured.");
        var secret = _configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("JWT secret key is not configured.");
        var tokenLifetimeMinutes = _configuration.GetValue("Jwt:TokenLifetimeMinutes", 60);
        var refreshLifetimeDays = _configuration.GetValue("Jwt:RefreshTokenLifetimeDays", 7);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString()),
        };

        if (user.CompanyId is Guid companyId)
        {
            claims.Add(new Claim(CustomClaimTypes.CompanyId, companyId.ToString()));
        }

        if (user.DriverId is Guid driverId)
        {
            claims.Add(new Claim(CustomClaimTypes.DriverId, driverId.ToString()));
        }

        var expiresAt = DateTime.UtcNow.AddMinutes(tokenLifetimeMinutes);

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: expiresAt,
            signingCredentials: credentials);

        var handler = new JwtSecurityTokenHandler();
        var accessToken = handler.WriteToken(token);

        var refreshTokenBytes = RandomNumberGenerator.GetBytes(32);
        var refreshToken = Convert.ToBase64String(refreshTokenBytes);
        var refreshExpiresAt = DateTime.UtcNow.AddDays(refreshLifetimeDays);

        return new AuthTokensDto(
            user.Id,
            user.Email,
            user.Role.ToString(),
            accessToken,
            expiresAt,
            refreshToken,
            refreshExpiresAt,
            user.CompanyId,
            user.DriverId);
    }
}
