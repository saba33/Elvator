using TransportLink.Application.Auth.DTOs;
using TransportLink.Domain.Entities;

namespace TransportLink.Application.Common.Interfaces;

public interface IJwtTokenService
{
    AuthTokensDto GenerateTokens(User user);
}
