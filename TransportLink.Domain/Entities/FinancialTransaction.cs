namespace TransportLink.Domain.Entities;

public sealed class FinancialTransaction
{
    private FinancialTransaction()
    {
    }

    public FinancialTransaction(Guid id, Guid paymentId, decimal amount, string description, DateTime createdAt)
    {
        Id = id;
        PaymentId = paymentId;
        Amount = amount;
        Description = description;
        CreatedAt = createdAt;
    }

    public Guid Id { get; init; }

    public Guid PaymentId { get; private set; }

    public Payment? Payment { get; private set; }

    public decimal Amount { get; private set; }

    public string Description { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }
}
