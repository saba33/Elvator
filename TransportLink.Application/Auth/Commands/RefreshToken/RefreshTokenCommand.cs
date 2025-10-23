using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TransportLink.Application.Auth.DTOs;
using TransportLink.Application.Common.Interfaces;
using TransportLink.Domain.Common;

namespace TransportLink.Application.Auth.Commands.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<Result<AuthTokensDto>>;

public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<AuthTokensDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtTokenService _jwtTokenService;

    public RefreshTokenCommandHandler(
        IApplicationDbContext context,
        IJwtTokenService jwtTokenService)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result<AuthTokensDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return Result<AuthTokensDto>.Failure("Refresh token is required.");
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken, cancellationToken);

        if (user is null)
        {
            return Result<AuthTokensDto>.Failure("Invalid refresh token.");
        }

        if (user.RefreshTokenExpiryTime is null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            user.ClearRefreshToken();
            await _context.SaveChangesAsync(cancellationToken);
            return Result<AuthTokensDto>.Failure("Refresh token has expired.");
        }

        var tokens = _jwtTokenService.GenerateTokens(user);
        user.SetRefreshToken(tokens.RefreshToken, tokens.RefreshTokenExpiresAt);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<AuthTokensDto>.Success(tokens);
    }
}
