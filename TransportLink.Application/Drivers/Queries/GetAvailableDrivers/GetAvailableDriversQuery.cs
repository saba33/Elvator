using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TransportLink.Application.Common.Interfaces;
using TransportLink.Application.Drivers.DTOs;
using TransportLink.Domain.Common;

namespace TransportLink.Application.Drivers.Queries.GetAvailableDrivers;

public sealed record GetAvailableDriversQuery : IRequest<Result<IReadOnlyCollection<DriverDto>>>;

public sealed class GetAvailableDriversQueryHandler : IRequestHandler<GetAvailableDriversQuery, Result<IReadOnlyCollection<DriverDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAvailableDriversQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<IReadOnlyCollection<DriverDto>>> Handle(GetAvailableDriversQuery request, CancellationToken cancellationToken)
    {
        var drivers = await _context.Drivers
            .AsNoTracking()
            .Where(driver => driver.IsAvailable)
            .OrderByDescending(driver => driver.Rating)
            .ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<DriverDto>>(drivers);
        return Result<IReadOnlyCollection<DriverDto>>.Success(dtos);
    }
}
