using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TransportLink.Application.Common.Interfaces;
using TransportLink.Application.Matching;
using TransportLink.Application.Orders.DTOs;
using TransportLink.Domain.Common;

namespace TransportLink.Application.Matching.Commands.AssignDriverToOrder;

public sealed record AssignDriverToOrderCommand(Guid OrderId) : IRequest<Result<OrderDto>>;

public sealed class AssignDriverToOrderCommandHandler : IRequestHandler<AssignDriverToOrderCommand, Result<OrderDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMatchingService _matchingService;
    private readonly IMapper _mapper;

    public AssignDriverToOrderCommandHandler(
        IApplicationDbContext context,
        IMatchingService matchingService,
        IMapper mapper)
    {
        _context = context;
        _matchingService = matchingService;
        _mapper = mapper;
    }

    public async Task<Result<OrderDto>> Handle(AssignDriverToOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .Include(o => o.Driver)
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if (order is null)
        {
            return Result<OrderDto>.Failure($"Order with id '{request.OrderId}' was not found.");
        }

        var requiredVehicle = _matchingService.SelectVehicleType(order.CargoType, order.WeightKg);

        var availableDrivers = await _context.Drivers
            .Where(driver => driver.IsAvailable && driver.VehicleType == requiredVehicle && driver.CompanyId == order.CompanyId)
            .ToListAsync(cancellationToken);

        if (availableDrivers.Count == 0)
        {
            return Result<OrderDto>.Failure("No available drivers match the requirements at this time.");
        }

        var bestDriver = _matchingService.FindBestDriver(order, availableDrivers);
        if (bestDriver is null)
        {
            return Result<OrderDto>.Failure("Unable to determine the best driver for the order.");
        }

        if (order.DriverId.HasValue && order.DriverId != bestDriver.Id)
        {
            var previousDriver = await _context.Drivers
                .FirstOrDefaultAsync(d => d.Id == order.DriverId.Value, cancellationToken);

            if (previousDriver is not null)
            {
                previousDriver.UpdateAvailability(true);
            }
        }

        order.AssignDriver(bestDriver.Id);
        bestDriver.AssignToOrder();

        await _context.SaveChangesAsync(cancellationToken);

        var resultOrder = await _context.Orders
            .AsNoTracking()
            .Include(o => o.Driver)
            .FirstAsync(o => o.Id == order.Id, cancellationToken);

        return Result<OrderDto>.Success(_mapper.Map<OrderDto>(resultOrder));
    }
}
