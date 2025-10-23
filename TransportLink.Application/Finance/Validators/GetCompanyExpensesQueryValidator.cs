using FluentValidation;
using TransportLink.Application.Finance.Queries.GetCompanyExpenses;

namespace TransportLink.Application.Finance.Validators;

public sealed class GetCompanyExpensesQueryValidator : AbstractValidator<GetCompanyExpensesQuery>
{
    public GetCompanyExpensesQueryValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty();

        RuleFor(x => x)
            .Must(range => !range.From.HasValue || !range.To.HasValue || range.From <= range.To)
            .WithMessage("The 'from' date must be earlier than or equal to the 'to' date.");
    }
}
