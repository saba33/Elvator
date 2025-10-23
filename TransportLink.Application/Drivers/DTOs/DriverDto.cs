using TransportLink.Domain.Enums;

namespace TransportLink.Application.Drivers.DTOs;

public sealed record DriverDto(
    Guid Id,
    Guid CompanyId,
    string Name,
    string Phone,
    decimal Rating,
    bool IsAvailable,
    VehicleType VehicleType,
    int ActiveAssignments);
