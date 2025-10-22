using TransportLink.Domain.Enums;

namespace TransportLink.Domain.Entities;

public sealed class Shipment
{
    private Shipment()
    {
    }

    public Shipment(Guid id, string reference, string origin, string destination, ShipmentStatus status, DateTime createdOn)
    {
        Id = id;
        Reference = reference;
        Origin = origin;
        Destination = destination;
        Status = status;
        CreatedOn = createdOn;
    }

    public Guid Id { get; init; }

    public string Reference { get; private set; } = string.Empty;

    public string Origin { get; private set; } = string.Empty;

    public string Destination { get; private set; } = string.Empty;

    public ShipmentStatus Status { get; private set; }

    public DateTime CreatedOn { get; private set; }

    public DateTime? UpdatedOn { get; private set; }

    public void UpdateStatus(ShipmentStatus status)
    {
        Status = status;
        UpdatedOn = DateTime.UtcNow;
    }
}
