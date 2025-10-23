using FluentValidation;
using TransportLink.Application.Finance.Queries.GetDriverPayouts;

namespace TransportLink.Application.Finance.Validators;

public sealed class GetDriverPayoutsQueryValidator : AbstractValidator<GetDriverPayoutsQuery>
{
    public GetDriverPayoutsQueryValidator()
    {
        RuleFor(x => x.DriverId)
            .NotEmpty();

        RuleFor(x => x)
            .Must(range => !range.From.HasValue || !range.To.HasValue || range.From <= range.To)
            .WithMessage("The 'from' date must be earlier than or equal to the 'to' date.");
    }
}
