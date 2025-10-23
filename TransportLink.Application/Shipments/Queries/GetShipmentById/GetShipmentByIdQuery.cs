using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TransportLink.Application.Common.Interfaces;
using TransportLink.Application.Shipments.DTOs;
using TransportLink.Domain.Common;

namespace TransportLink.Application.Shipments.Queries.GetShipmentById;

public sealed record GetShipmentByIdQuery(Guid ShipmentId) : IRequest<Result<ShipmentDto>>;

public sealed class GetShipmentByIdQueryHandler : IRequestHandler<GetShipmentByIdQuery, Result<ShipmentDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetShipmentByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<ShipmentDto>> Handle(GetShipmentByIdQuery request, CancellationToken cancellationToken)
    {
        var shipment = await _context.Shipments
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == request.ShipmentId, cancellationToken);

        if (shipment is null)
        {
            return Result<ShipmentDto>.Failure($"Shipment with id '{request.ShipmentId}' was not found.");
        }

        return Result<ShipmentDto>.Success(_mapper.Map<ShipmentDto>(shipment));
    }
}
