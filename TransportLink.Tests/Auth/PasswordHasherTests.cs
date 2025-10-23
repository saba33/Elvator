using TransportLink.Infrastructure.Services;
using Xunit;

namespace TransportLink.Tests.Auth;

public sealed class PasswordHasherTests
{
    private readonly PasswordHasher _hasher = new();

    [Fact]
    public void HashAndVerify_Succeeds_ForCorrectPassword()
    {
        const string password = "StrongPassword123!";

        var hashed = _hasher.HashPassword(password);

        Assert.True(_hasher.VerifyHashedPassword(hashed, password));
    }

    [Fact]
    public void VerifyHashedPassword_Fails_ForIncorrectPassword()
    {
        const string password = "AnotherStrongPassword123!";
        const string wrongPassword = "WrongPassword456!";

        var hashed = _hasher.HashPassword(password);

        Assert.False(_hasher.VerifyHashedPassword(hashed, wrongPassword));
    }
}
