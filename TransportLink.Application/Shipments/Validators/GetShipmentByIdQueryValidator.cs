using FluentValidation;
using TransportLink.Application.Shipments.Queries.GetShipmentById;

namespace TransportLink.Application.Shipments.Validators;

public sealed class GetShipmentByIdQueryValidator : AbstractValidator<GetShipmentByIdQuery>
{
    public GetShipmentByIdQueryValidator()
    {
        RuleFor(q => q.ShipmentId)
            .NotEmpty();
    }
}
