using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportLink.Domain.Entities;

namespace TransportLink.Infrastructure.Configurations;

public sealed class FinancialTransactionConfiguration : IEntityTypeConfiguration<FinancialTransaction>
{
    public void Configure(EntityTypeBuilder<FinancialTransaction> builder)
    {
        builder.ToTable("financial_transactions", "finance");

        builder.HasKey(transaction => transaction.Id);

        builder.Property(transaction => transaction.Amount)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(transaction => transaction.Description)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(transaction => transaction.CreatedAt)
            .IsRequired();

        builder.HasOne(transaction => transaction.Payment)
            .WithMany(payment => payment.Transactions)
            .HasForeignKey(transaction => transaction.PaymentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(transaction => transaction.PaymentId);
        builder.HasIndex(transaction => transaction.CreatedAt);
    }
}
