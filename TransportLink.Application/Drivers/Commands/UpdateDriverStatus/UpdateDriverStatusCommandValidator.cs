using FluentValidation;

namespace TransportLink.Application.Drivers.Commands.UpdateDriverStatus;

public sealed class UpdateDriverStatusCommandValidator : AbstractValidator<UpdateDriverStatusCommand>
{
    public UpdateDriverStatusCommandValidator()
    {
        RuleFor(command => command.DriverId)
            .NotEmpty();
    }
}
