using TransportLink.Domain.Enums;

namespace TransportLink.Domain.Entities;

public sealed class Order
{
    private Order()
    {
    }

    public Order(
        Guid id,
        Guid companyId,
        string cargoType,
        decimal weightKg,
        decimal price,
        OrderStatus status,
        DateTime createdAt,
        DateTime updatedAt,
        Guid? driverId = null)
    {
        Id = id;
        CompanyId = companyId;
        CargoType = cargoType;
        WeightKg = weightKg;
        Price = price;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        DriverId = driverId;
    }

    public Guid Id { get; init; }

    public Guid CompanyId { get; private set; }

    public string CargoType { get; private set; } = string.Empty;

    public decimal WeightKg { get; private set; }

    public decimal Price { get; private set; }

    public OrderStatus Status { get; private set; }

    public Guid? DriverId { get; private set; }

    public Driver? Driver { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public void UpdateStatus(OrderStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AssignDriver(Guid driverId)
    {
        DriverId = driverId;
        Status = OrderStatus.Assigned;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ClearDriver()
    {
        DriverId = null;
        Status = OrderStatus.Pending;
        UpdatedAt = DateTime.UtcNow;
    }
}
