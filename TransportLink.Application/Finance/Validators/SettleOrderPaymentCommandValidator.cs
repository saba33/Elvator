using FluentValidation;
using TransportLink.Application.Finance.Commands.SettleOrderPayment;

namespace TransportLink.Application.Finance.Validators;

public sealed class SettleOrderPaymentCommandValidator : AbstractValidator<SettleOrderPaymentCommand>
{
    public SettleOrderPaymentCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty();
    }
}
