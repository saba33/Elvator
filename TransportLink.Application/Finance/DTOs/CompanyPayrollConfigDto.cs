using TransportLink.Domain.Enums;

namespace TransportLink.Application.Finance.DTOs;

public sealed record CompanyPayrollConfigDto(
    Guid CompanyId,
    SalaryMode SalaryMode,
    decimal? PerOrderRatePct,
    decimal? DailyRate,
    decimal? MonthlySalary,
    DateTime CreatedAt);
