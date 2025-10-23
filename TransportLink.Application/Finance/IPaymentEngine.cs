using System.Collections.Generic;
using TransportLink.Domain.Common;
using TransportLink.Domain.Entities;

namespace TransportLink.Application.Finance;

public interface IPaymentEngine
{
    decimal CalculateForOrder(Order order, Driver driver, CompanyPayrollConfig companyConfig);

    Task<Result<IReadOnlyCollection<Payment>>> SettleOrderPaymentAsync(Guid orderId, CancellationToken cancellationToken = default);
}
