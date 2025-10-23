using FluentValidation;
using TransportLink.Application.Orders.Commands.CreateOrder;

namespace TransportLink.Application.Orders.Validators;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(c => c.CompanyId)
            .NotEmpty();

        RuleFor(c => c.CargoType)
            .NotEmpty();

        RuleFor(c => c.WeightKg)
            .GreaterThan(0);

        RuleFor(c => c.Price)
            .GreaterThanOrEqualTo(0);
    }
}
