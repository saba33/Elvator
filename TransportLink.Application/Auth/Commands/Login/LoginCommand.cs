using MediatR;
using Microsoft.EntityFrameworkCore;
using TransportLink.Application.Auth.DTOs;
using TransportLink.Application.Common.Interfaces;
using TransportLink.Domain.Common;

namespace TransportLink.Application.Auth.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<Result<AuthTokensDto>>;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthTokensDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result<AuthTokensDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == normalizedEmail, cancellationToken);

        if (user is null)
        {
            return Result<AuthTokensDto>.Failure("Invalid credentials.");
        }

        var validPassword = _passwordHasher.VerifyHashedPassword(user.PasswordHash, request.Password);
        if (!validPassword)
        {
            return Result<AuthTokensDto>.Failure("Invalid credentials.");
        }

        var tokens = _jwtTokenService.GenerateTokens(user);
        user.SetRefreshToken(tokens.RefreshToken, tokens.RefreshTokenExpiresAt);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<AuthTokensDto>.Success(tokens);
    }
}
