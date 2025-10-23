using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TransportLink.Application.Common.Interfaces;
using TransportLink.Application.Finance;
using TransportLink.Application.Matching;
using TransportLink.Infrastructure.Persistence;
using TransportLink.Infrastructure.Services;

namespace TransportLink.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Database connection string 'Database' is not configured.");
        }

        services.AddDbContext<TransportLinkDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<TransportLinkDbContext>());
        services.AddScoped<IMatchingService, MatchingService>();
        services.AddScoped<IPaymentEngine, PaymentEngine>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();

        return services;
    }
}
