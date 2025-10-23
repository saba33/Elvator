using FluentValidation;
using TransportLink.Application.Finance.Commands.ConfigureCompanyPayroll;
using TransportLink.Domain.Enums;

namespace TransportLink.Application.Finance.Validators;

public sealed class ConfigureCompanyPayrollCommandValidator : AbstractValidator<ConfigureCompanyPayrollCommand>
{
    public ConfigureCompanyPayrollCommandValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty();

        RuleFor(x => x.SalaryMode)
            .IsInEnum();

        When(x => x.SalaryMode == SalaryMode.PerOrder, () =>
        {
            RuleFor(x => x.PerOrderRatePct)
                .NotNull()
                .GreaterThan(0m)
                .LessThanOrEqualTo(1m);
        });

        When(x => x.SalaryMode == SalaryMode.Daily, () =>
        {
            RuleFor(x => x.DailyRate)
                .NotNull()
                .GreaterThan(0m);
        });

        When(x => x.SalaryMode == SalaryMode.Monthly || x.SalaryMode == SalaryMode.Fixed, () =>
        {
            RuleFor(x => x.MonthlySalary)
                .NotNull()
                .GreaterThan(0m);
        });
    }
}
