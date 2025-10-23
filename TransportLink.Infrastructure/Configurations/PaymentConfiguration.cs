using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportLink.Domain.Entities;

namespace TransportLink.Infrastructure.Configurations;

public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments", "finance");

        builder.HasKey(payment => payment.Id);

        builder.Property(payment => payment.Amount)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(payment => payment.Method)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(payment => payment.Status)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(payment => payment.CreatedAt)
            .IsRequired();

        builder.HasOne(payment => payment.Order)
            .WithMany()
            .HasForeignKey(payment => payment.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(payment => payment.Driver)
            .WithMany(driver => driver.Payments)
            .HasForeignKey(payment => payment.DriverId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(payment => payment.Company)
            .WithMany(company => company.Payments)
            .HasForeignKey(payment => payment.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(payment => payment.OrderId);
        builder.HasIndex(payment => payment.DriverId);
        builder.HasIndex(payment => payment.CompanyId);
    }
}
