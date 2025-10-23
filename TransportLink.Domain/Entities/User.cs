using System;
using TransportLink.Domain.Enums;

namespace TransportLink.Domain.Entities;

public sealed class User
{
    private User()
    {
    }

    public User(
        Guid id,
        string email,
        string passwordHash,
        UserRole role,
        DateTime createdAt,
        Guid? companyId = null,
        Guid? driverId = null)
    {
        Id = id;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        CompanyId = companyId;
        DriverId = driverId;
        CreatedAt = createdAt;
    }

    public Guid Id { get; init; }

    public string Email { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public UserRole Role { get; private set; }

    public Guid? CompanyId { get; private set; }

    public Guid? DriverId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public string? RefreshToken { get; private set; }

    public DateTime? RefreshTokenExpiryTime { get; private set; }

    public void SetRefreshToken(string refreshToken, DateTime expiresAt)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = expiresAt;
    }

    public void ClearRefreshToken()
    {
        RefreshToken = null;
        RefreshTokenExpiryTime = null;
    }
}
