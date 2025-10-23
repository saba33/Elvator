using System.Collections.Generic;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TransportLink.Application.Common.Interfaces;
using TransportLink.Application.Finance.DTOs;
using TransportLink.Domain.Common;

namespace TransportLink.Application.Finance.Queries.GetCompanyExpenses;

public sealed record GetCompanyExpensesQuery(Guid CompanyId, DateTime? From, DateTime? To) : IRequest<Result<IReadOnlyCollection<PaymentDto>>>;

public sealed class GetCompanyExpensesQueryHandler : IRequestHandler<GetCompanyExpensesQuery, Result<IReadOnlyCollection<PaymentDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCompanyExpensesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<IReadOnlyCollection<PaymentDto>>> Handle(GetCompanyExpensesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Payments
            .AsNoTracking()
            .Where(payment => payment.CompanyId == request.CompanyId);

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
