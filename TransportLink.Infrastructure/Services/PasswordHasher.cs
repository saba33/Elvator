using System;
using System.Security.Cryptography;
using TransportLink.Application.Common.Interfaces;

namespace TransportLink.Infrastructure.Services;

public sealed class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 100_000;
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

    public string HashPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Password must be provided.", nameof(password));
        }

        Span<byte> salt = stackalloc byte[SaltSize];
        RandomNumberGenerator.Fill(salt);

        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, KeySize);

        return string.Join(
            '.',
            Iterations.ToString(),
            Convert.ToBase64String(salt),
            Convert.ToBase64String(hash));
    }

    public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        if (string.IsNullOrWhiteSpace(hashedPassword) || string.IsNullOrEmpty(providedPassword))
        {
            return false;
        }

        var segments = hashedPassword.Split('.', 3);
        if (segments.Length != 3 || !int.TryParse(segments[0], out var iterations))
        {
            return false;
        }

        var salt = Convert.FromBase64String(segments[1]);
        var storedHash = Convert.FromBase64String(segments[2]);

        var computedHash = Rfc2898DeriveBytes.Pbkdf2(providedPassword, salt, iterations, Algorithm, storedHash.Length);

        return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
    }
}
