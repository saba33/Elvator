using TransportLink.Domain.Enums;

namespace TransportLink.Api.Contracts.Finance;

public sealed record ConfigureCompanyPayrollRequest(
    Guid CompanyId,
    SalaryMode SalaryMode,
    decimal? PerOrderRatePct,
    decimal? DailyRate,
    decimal? MonthlySalary);
