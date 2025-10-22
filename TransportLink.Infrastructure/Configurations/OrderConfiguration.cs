using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportLink.Domain.Entities;

namespace TransportLink.Infrastructure.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(order => order.Id);

        builder.Property(order => order.CompanyId)
            .IsRequired();

        builder.Property(order => order.DriverId);

        builder.HasOne(order => order.Driver)
            .WithMany(driver => driver.Orders)
            .HasForeignKey(order => order.DriverId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(order => order.DriverId);

        builder.Property(order => order.CargoType)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(order => order.WeightKg)
            .IsRequired()
            .HasColumnType("numeric(18,2)");

        builder.Property(order => order.Price)
            .IsRequired()
            .HasColumnType("numeric(18,2)");

        builder.Property(order => order.Status)
            .IsRequired();

        builder.Property(order => order.CreatedAt)
            .IsRequired();

        builder.Property(order => order.UpdatedAt)
            .IsRequired();
    }
}
