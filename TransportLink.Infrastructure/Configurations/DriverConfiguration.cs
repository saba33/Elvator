using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportLink.Domain.Entities;
using TransportLink.Domain.Enums;

namespace TransportLink.Infrastructure.Configurations;

public sealed class DriverConfiguration : IEntityTypeConfiguration<Driver>
{
    private static readonly Guid DriverAliceId = Guid.Parse("3c7d5a4f-54b4-4f48-8b1e-7058ff5a9c11");
    private static readonly Guid DriverBobId = Guid.Parse("5ab7136f-0c1a-4b60-8d13-59c9c7bd08bf");
    private static readonly Guid DriverCarlosId = Guid.Parse("f47b4b82-7fb3-4c36-9e39-2adce37b329e");

    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.ToTable("drivers");

        builder.HasKey(driver => driver.Id);

        builder.Property(driver => driver.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(driver => driver.Phone)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(driver => driver.Rating)
            .HasColumnType("numeric(3,2)")
            .HasDefaultValue(0);

        builder.Property(driver => driver.IsAvailable)
            .IsRequired();

        builder.Property(driver => driver.VehicleType)
            .IsRequired();

        builder.Property(driver => driver.ActiveAssignments)
            .HasDefaultValue(0);

        builder.HasData(
            new Driver(DriverAliceId, CompanyConfiguration.LogisticsHubId, "Alice Walker", "+1-555-0101", 4.8m, true, VehicleType.Truck),
            new Driver(DriverBobId, CompanyConfiguration.LogisticsHubId, "Bob Nguyen", "+1-555-0102", 4.5m, true, VehicleType.Van),
            new Driver(DriverCarlosId, CompanyConfiguration.ExpressFreightId, "Carlos Diaz", "+1-555-0103", 4.9m, true, VehicleType.Pickup));
    }
}
