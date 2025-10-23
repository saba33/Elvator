using FluentValidation;

namespace TransportLink.Application.Matching.Commands.AssignDriverToOrder;

public sealed class AssignDriverToOrderCommandValidator : AbstractValidator<AssignDriverToOrderCommand>
{
    public AssignDriverToOrderCommandValidator()
    {
        RuleFor(command => command.OrderId)
            .NotEmpty();
    }
}
