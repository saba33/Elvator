using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TransportLink.Application.Common.Interfaces;
using TransportLink.Application.Drivers.DTOs;
using TransportLink.Domain.Common;

namespace TransportLink.Application.Drivers.Commands.UpdateDriverStatus;

public sealed record UpdateDriverStatusCommand(Guid DriverId, bool IsAvailable) : IRequest<Result<DriverDto>>;

public sealed class UpdateDriverStatusCommandHandler : IRequestHandler<UpdateDriverStatusCommand, Result<DriverDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateDriverStatusCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<DriverDto>> Handle(UpdateDriverStatusCommand request, CancellationToken cancellationToken)
    {
        var driver = await _context.Drivers
            .FirstOrDefaultAsync(d => d.Id == request.DriverId, cancellationToken);

        if (driver is null)
        {
            return Result<DriverDto>.Failure($"Driver with id '{request.DriverId}' was not found.");
        }

        driver.UpdateAvailability(request.IsAvailable);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<DriverDto>.Success(_mapper.Map<DriverDto>(driver));
    }
}
