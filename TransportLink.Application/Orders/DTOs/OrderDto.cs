using TransportLink.Domain.Enums;

namespace TransportLink.Application.Orders.DTOs;

public sealed record OrderDto(
    Guid Id,
    Guid CompanyId,
    string CargoType,
    decimal WeightKg,
    decimal Price,
    OrderStatus Status,
    Guid? DriverId,
    string? DriverName,
    VehicleType? VehicleType,
    DateTime CreatedAt,
    DateTime UpdatedAt);
