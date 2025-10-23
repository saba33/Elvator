using System.Collections.Generic;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TransportLink.Application.Common.Interfaces;
using TransportLink.Application.Finance.DTOs;
using TransportLink.Domain.Common;

namespace TransportLink.Application.Finance.Queries.GetDriverPayouts;

public sealed record GetDriverPayoutsQuery(Guid DriverId, DateTime? From, DateTime? To) : IRequest<Result<IReadOnlyCollection<PaymentDto>>>;

public sealed class GetDriverPayoutsQueryHandler : IRequestHandler<GetDriverPayoutsQuery, Result<IReadOnlyCollection<PaymentDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDriverPayoutsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<IReadOnlyCollection<PaymentDto>>> Handle(GetDriverPayoutsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Payments
            .AsNoTracking()
            .Where(payment => payment.DriverId == request.DriverId);

        if (request.From.HasValue)
        {
            query = query.Where(payment => payment.CreatedAt >= request.From.Value);
        }

        if (request.To.HasValue)
        {
            query = query.Where(payment => payment.CreatedAt <= request.To.Value);
        }

        var payments = await query
            .OrderBy(payment => payment.CreatedAt)
            .ToListAsync(cancellationToken);

        var mapped = _mapper.Map<List<PaymentDto>>(payments);
        return Result<IReadOnlyCollection<PaymentDto>>.Success(mapped);
    }
}
