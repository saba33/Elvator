using System.Collections.Generic;

namespace TransportLink.Domain.Entities;

public sealed class Payment
{
    private Payment()
    {
    }

    public Payment(
        Guid id,
        Guid orderId,
        Guid? driverId,
        Guid companyId,
        decimal amount,
        string method,
        string status,
        DateTime createdAt)
    {
        Id = id;
        OrderId = orderId;
        DriverId = driverId;
        CompanyId = companyId;
        Amount = amount;
        Method = method;
        Status = status;
        CreatedAt = createdAt;
    }

    public Guid Id { get; init; }

    public Guid OrderId { get; private set; }

    public Order? Order { get; private set; }

    public Guid? DriverId { get; private set; }

    public Driver? Driver { get; private set; }

    public Guid CompanyId { get; private set; }

    public Company? Company { get; private set; }

    public decimal Amount { get; private set; }

    public string Method { get; private set; } = string.Empty;

    public string Status { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    public ICollection<FinancialTransaction> Transactions { get; private set; } = new List<FinancialTransaction>();
}
