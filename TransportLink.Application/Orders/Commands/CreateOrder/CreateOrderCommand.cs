using AutoMapper;
using MediatR;
using TransportLink.Application.Common.Interfaces;
using TransportLink.Application.Orders.DTOs;
using TransportLink.Domain.Common;
using TransportLink.Domain.Entities;
using TransportLink.Domain.Enums;

namespace TransportLink.Application.Orders.Commands.CreateOrder;

public sealed record CreateOrderCommand(
    Guid CompanyId,
    string CargoType,
    decimal WeightKg,
    decimal Price) : IRequest<Result<OrderDto>>;

public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<OrderDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateOrderCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<OrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var timestamp = DateTime.UtcNow;
        var order = new Order(
            Guid.NewGuid(),
            request.CompanyId,
            request.CargoType,
            request.WeightKg,
            request.Price,
            OrderStatus.Pending,
            timestamp,
            timestamp);

        await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<OrderDto>.Success(_mapper.Map<OrderDto>(order));
    }
}
