using TransportLink.Domain.Enums;

namespace TransportLink.Application.Shipments.DTOs;

public sealed record ShipmentDto(Guid Id, string Reference, string Origin, string Destination, ShipmentStatus Status, DateTime CreatedOn);
