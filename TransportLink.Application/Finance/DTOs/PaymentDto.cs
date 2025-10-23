namespace TransportLink.Application.Finance.DTOs;

public sealed record PaymentDto(
    Guid Id,
    Guid OrderId,
    Guid? DriverId,
    Guid CompanyId,
    decimal Amount,
    string Method,
    string Status,
    DateTime CreatedAt);
