using FluentValidation;
using TransportLink.Application.Auth.Commands.RegisterCompanyManager;

namespace TransportLink.Application.Auth.Validators;

public sealed class RegisterCompanyManagerCommandValidator : AbstractValidator<RegisterCompanyManagerCommand>
{
    public RegisterCompanyManagerCommandValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8);
    }
}
