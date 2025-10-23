using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportLink.Domain.Entities;
using TransportLink.Domain.Enums;

namespace TransportLink.Infrastructure.Configurations;

public sealed class CompanyPayrollConfigConfiguration : IEntityTypeConfiguration<CompanyPayrollConfig>
{
    private static readonly DateTime SeedCreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public void Configure(EntityTypeBuilder<CompanyPayrollConfig> builder)
    {
        builder.ToTable("company_payroll_configs", "finance");

        builder.HasKey(config => config.CompanyId);

        builder.Property(config => config.SalaryMode)
            .IsRequired();

        builder.Property(config => config.PerOrderRatePct)
            .HasColumnType("numeric(5,4)");

        builder.Property(config => config.DailyRate)
            .HasColumnType("numeric(18,2)");

        builder.Property(config => config.MonthlySalary)
            .HasColumnType("numeric(18,2)");

        builder.Property(config => config.CreatedAt)
            .IsRequired();

        builder.HasOne(config => config.Company)
            .WithOne(company => company.PayrollConfig)
            .HasForeignKey<CompanyPayrollConfig>(config => config.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(
            new CompanyPayrollConfig(
                CompanyConfiguration.LogisticsHubId,
                SalaryMode.PerOrder,
                0.7000m,
                null,
                null,
                SeedCreatedAt));
    }
}
