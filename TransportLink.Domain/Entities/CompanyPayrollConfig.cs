using TransportLink.Domain.Enums;

namespace TransportLink.Domain.Entities;

public sealed class CompanyPayrollConfig
{
    private CompanyPayrollConfig()
    {
    }

    public CompanyPayrollConfig(
        Guid companyId,
        SalaryMode salaryMode,
        decimal? perOrderRatePct,
        decimal? dailyRate,
        decimal? monthlySalary,
        DateTime createdAt)
    {
        CompanyId = companyId;
        SalaryMode = salaryMode;
        PerOrderRatePct = perOrderRatePct;
        DailyRate = dailyRate;
        MonthlySalary = monthlySalary;
        CreatedAt = createdAt;
    }

    public Guid CompanyId { get; init; }

    public Company? Company { get; private set; }

    public SalaryMode SalaryMode { get; private set; }

    public decimal? PerOrderRatePct { get; private set; }

    public decimal? DailyRate { get; private set; }

    public decimal? MonthlySalary { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public void UpdateConfiguration(
        SalaryMode salaryMode,
        decimal? perOrderRatePct,
        decimal? dailyRate,
        decimal? monthlySalary)
    {
        SalaryMode = salaryMode;
        PerOrderRatePct = perOrderRatePct;
        DailyRate = dailyRate;
        MonthlySalary = monthlySalary;
        CreatedAt = DateTime.UtcNow;
    }
}
