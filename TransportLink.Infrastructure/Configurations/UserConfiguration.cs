using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportLink.Domain.Entities;
using TransportLink.Domain.Enums;

namespace TransportLink.Infrastructure.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public static readonly Guid AdminUserId = Guid.Parse("9f1f19b2-30cb-4af5-8f0f-0bf0a9123c0d");

    private const string AdminPasswordHash = "100000.1Uw9X49vRWe/K4Rh9N1h8w==.4/mMh6e3W6IDDI3TgXiMkMrGvBDobecENb2KDclcIsI=";

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasIndex(user => user.Email)
            .IsUnique();

        builder.HasIndex(user => user.CompanyId);

        builder.Property(user => user.PasswordHash)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(user => user.Role)
            .IsRequired();

        builder.Property(user => user.CreatedAt)
            .IsRequired();

        builder.Property(user => user.RefreshToken)
            .HasMaxLength(512);

        builder.HasIndex(user => user.DriverId)
            .IsUnique()
            .HasFilter("\"DriverId\" IS NOT NULL");

        builder.HasOne<Company>()
            .WithMany()
            .HasForeignKey(user => user.CompanyId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<Driver>()
            .WithMany()
            .HasForeignKey(user => user.DriverId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasData(
            new User(
                AdminUserId,
                "admin@transportlink.io",
                AdminPasswordHash,
                UserRole.Admin,
                new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)));
    }
}
