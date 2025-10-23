using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TransportLink.Application.Auth.DTOs;
using TransportLink.Application.Common.Interfaces;
using TransportLink.Domain.Common;
using TransportLink.Domain.Entities;
using TransportLink.Domain.Enums;

namespace TransportLink.Application.Auth.Commands.RegisterDriver;

public sealed record RegisterDriverCommand(Guid DriverId, string Email, string Password)
    : IRequest<Result<AuthTokensDto>>;

public sealed class RegisterDriverCommandHandler : IRequestHandler<RegisterDriverCommand, Result<AuthTokensDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public RegisterDriverCommandHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result<AuthTokensDto>> Handle(RegisterDriverCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var driver = await _context.Drivers
            .FirstOrDefaultAsync(d => d.Id == request.DriverId, cancellationToken);

        if (driver is null)
        {
            return Result<AuthTokensDto>.Failure("Driver not found.");
        }

        var emailInUse = await _context.Users
            .AnyAsync(user => user.Email == normalizedEmail, cancellationToken);

        if (emailInUse)
        {
            return Result<AuthTokensDto>.Failure("Email is already registered.");
        }

        var driverAlreadyLinked = await _context.Users
            .AnyAsync(user => user.DriverId == driver.Id, cancellationToken);

        if (driverAlreadyLinked)
        {
            return Result<AuthTokensDto>.Failure("Driver already has an associated user account.");
        }

        var timestamp = DateTime.UtcNow;
        var user = new User(
            Guid.NewGuid(),
            normalizedEmail,
            _passwordHasher.HashPassword(request.Password),
            UserRole.Driver,
            timestamp,
            driver.CompanyId,
            driver.Id);

        var tokens = _jwtTokenService.GenerateTokens(user);
        user.SetRefreshToken(tokens.RefreshToken, tokens.RefreshTokenExpiresAt);

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<AuthTokensDto>.Success(tokens);
    }
}
