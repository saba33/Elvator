using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportLink.Domain.Entities;

namespace TransportLink.Infrastructure.Configurations;

public sealed class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public static readonly Guid LogisticsHubId = Guid.Parse("4b1a0f97-98f5-4d4d-9e9c-948d1edb5b70");
    public static readonly Guid ExpressFreightId = Guid.Parse("c31a68a0-8ff1-4020-8bd2-1651a8651adb");

    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("companies");

        builder.HasKey(company => company.Id);

        builder.Property(company => company.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(company => company.Address)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(company => company.ContactEmail)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasMany(company => company.Drivers)
            .WithOne(driver => driver.Company)
            .HasForeignKey(driver => driver.CompanyId);

        builder.HasData(
            new Company(LogisticsHubId, "Logistics Hub", "100 Market Street, Springfield", "contact@logisticshub.com"),
            new Company(ExpressFreightId, "Express Freight", "200 Cargo Road, Metropolis", "ops@expressfreight.io"));
    }
}
