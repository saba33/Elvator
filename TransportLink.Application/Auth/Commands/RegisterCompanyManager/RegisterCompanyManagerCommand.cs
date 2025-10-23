using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TransportLink.Application.Auth.DTOs;
using TransportLink.Application.Common.Interfaces;
using TransportLink.Domain.Common;
using TransportLink.Domain.Entities;
using TransportLink.Domain.Enums;

namespace TransportLink.Application.Auth.Commands.RegisterCompanyManager;

public sealed record RegisterCompanyManagerCommand(Guid CompanyId, string Email, string Password)
    : IRequest<Result<AuthTokensDto>>;

public sealed class RegisterCompanyManagerCommandHandler
    : IRequestHandler<RegisterCompanyManagerCommand, Result<AuthTokensDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public RegisterCompanyManagerCommandHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result<AuthTokensDto>> Handle(RegisterCompanyManagerCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var companyExists = await _context.Companies
            .AnyAsync(company => company.Id == request.CompanyId, cancellationToken);

        if (!companyExists)
        {
            return Result<AuthTokensDto>.Failure("Company not found.");
        }

        var emailInUse = await _context.Users
            .AnyAsync(user => user.Email == normalizedEmail, cancellationToken);

        if (emailInUse)
        {
            return Result<AuthTokensDto>.Failure("Email is already registered.");
        }

        var timestamp = DateTime.UtcNow;
        var user = new User(
            Guid.NewGuid(),
            normalizedEmail,
            _passwordHasher.HashPassword(request.Password),
            UserRole.CompanyManager,
            timestamp,
            request.CompanyId);

        var tokens = _jwtTokenService.GenerateTokens(user);
        user.SetRefreshToken(tokens.RefreshToken, tokens.RefreshTokenExpiresAt);

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<AuthTokensDto>.Success(tokens);
    }
}
