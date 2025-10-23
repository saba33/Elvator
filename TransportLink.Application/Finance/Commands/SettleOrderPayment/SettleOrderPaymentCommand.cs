using System.Collections.Generic;
using AutoMapper;
using MediatR;
using TransportLink.Application.Finance;
using TransportLink.Application.Finance.DTOs;
using TransportLink.Domain.Common;

namespace TransportLink.Application.Finance.Commands.SettleOrderPayment;

public sealed record SettleOrderPaymentCommand(Guid OrderId) : IRequest<Result<IReadOnlyCollection<PaymentDto>>>;

public sealed class SettleOrderPaymentCommandHandler : IRequestHandler<SettleOrderPaymentCommand, Result<IReadOnlyCollection<PaymentDto>>>
{
    private readonly IPaymentEngine _paymentEngine;
    private readonly IMapper _mapper;

    public SettleOrderPaymentCommandHandler(IPaymentEngine paymentEngine, IMapper mapper)
    {
        _paymentEngine = paymentEngine;
        _mapper = mapper;
    }

    public async Task<Result<IReadOnlyCollection<PaymentDto>>> Handle(SettleOrderPaymentCommand request, CancellationToken cancellationToken)
    {
        var settlementResult = await _paymentEngine.SettleOrderPaymentAsync(request.OrderId, cancellationToken);

        if (settlementResult.IsFailure || settlementResult.Value is null)
        {
            return Result<IReadOnlyCollection<PaymentDto>>.Failure(settlementResult.Error ?? "Unable to settle order payment.");
        }

        var payload = _mapper.Map<List<PaymentDto>>(settlementResult.Value);
        return Result<IReadOnlyCollection<PaymentDto>>.Success(payload);
    }
}
