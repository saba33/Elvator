using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TransportLink.Application.Common.Interfaces;
using TransportLink.Application.Finance;
using TransportLink.Domain.Common;
using TransportLink.Domain.Entities;
using TransportLink.Domain.Enums;

namespace TransportLink.Infrastructure.Services;

public sealed class PaymentEngine : IPaymentEngine
{
    private readonly IApplicationDbContext _context;

    public PaymentEngine(IApplicationDbContext context)
    {
        _context = context;
    }

    public decimal CalculateForOrder(Order order, Driver driver, CompanyPayrollConfig companyConfig)
    {
        _ = driver;

        return companyConfig.SalaryMode switch
        {
            SalaryMode.PerOrder => CalculatePerOrder(order, companyConfig),
            SalaryMode.Daily => ValidatePositive(companyConfig.DailyRate, "Daily rate must be configured for daily salary mode."),
            SalaryMode.Monthly => ValidatePositive(companyConfig.MonthlySalary, "Monthly salary must be configured for monthly salary mode."),
            SalaryMode.Fixed => ValidatePositive(companyConfig.MonthlySalary, "Monthly salary must be configured for fixed salary mode."),
            _ => throw new InvalidOperationException("Unsupported salary mode.")
        };
    }

    public async Task<Result<IReadOnlyCollection<Payment>>> SettleOrderPaymentAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        if (order is null)
        {
            return Result<IReadOnlyCollection<Payment>>.Failure("Order not found.");
        }

        if (order.Status != OrderStatus.Delivered)
        {
            return Result<IReadOnlyCollection<Payment>>.Failure("Only delivered orders can be settled.");
        }

        if (!order.DriverId.HasValue)
        {
            return Result<IReadOnlyCollection<Payment>>.Failure("Order must have an assigned driver before settlement.");
        }

        var driver = await _context.Drivers
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == order.DriverId.Value, cancellationToken);

        if (driver is null)
        {
            return Result<IReadOnlyCollection<Payment>>.Failure("Assigned driver not found.");
        }

        var configuration = await _context.CompanyPayrollConfigs
            .AsNoTracking()
            .FirstOrDefaultAsync(config => config.CompanyId == order.CompanyId, cancellationToken);

        if (configuration is null)
        {
            return Result<IReadOnlyCollection<Payment>>.Failure("Company payroll configuration not found.");
        }

        var alreadySettled = await _context.Payments
            .AsNoTracking()
            .AnyAsync(payment => payment.OrderId == orderId, cancellationToken);

        if (alreadySettled)
        {
            return Result<IReadOnlyCollection<Payment>>.Failure("Payments for this order have already been settled.");
        }

        decimal driverAmount;

        try
        {
            driverAmount = CalculateForOrder(order, driver, configuration);
        }
        catch (InvalidOperationException exception)
        {
            return Result<IReadOnlyCollection<Payment>>.Failure(exception.Message);
        }

        if (driverAmount <= 0m)
        {
            return Result<IReadOnlyCollection<Payment>>.Failure("Calculated driver payout must be greater than zero.");
        }

        var payments = new List<Payment>();
        var transactions = new List<FinancialTransaction>();
        var timestamp = DateTime.UtcNow;

        var roundedDriverAmount = RoundCurrency(driverAmount);
        var driverPayment = new Payment(
            Guid.NewGuid(),
            order.Id,
            driver.Id,
            order.CompanyId,
            roundedDriverAmount,
            "Payroll",
            "Settled",
            timestamp);

        payments.Add(driverPayment);
        transactions.Add(CreateTransaction(driverPayment, order.Id, timestamp));

        var platformFee = configuration.SalaryMode == SalaryMode.PerOrder
            ? RoundCurrency(Math.Max(0m, order.Price - roundedDriverAmount))
            : 0m;

        if (platformFee > 0m)
        {
            var platformPayment = new Payment(
                Guid.NewGuid(),
                order.Id,
                null,
                order.CompanyId,
                platformFee,
                "PlatformFee",
                "Settled",
                timestamp);

            payments.Add(platformPayment);
            transactions.Add(CreateTransaction(platformPayment, order.Id, timestamp));
        }

        await _context.Payments.AddRangeAsync(payments, cancellationToken);
        await _context.FinancialTransactions.AddRangeAsync(transactions, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<IReadOnlyCollection<Payment>>.Success(payments);
    }

    private static decimal CalculatePerOrder(Order order, CompanyPayrollConfig configuration)
    {
        if (configuration.PerOrderRatePct is null)
        {
            throw new InvalidOperationException("Per-order rate must be configured for per-order salary mode.");
        }

        if (configuration.PerOrderRatePct <= 0m || configuration.PerOrderRatePct > 1m)
        {
            throw new InvalidOperationException("Per-order rate must be between 0 and 1.");
        }

        return RoundCurrency(order.Price * configuration.PerOrderRatePct.Value);
    }

    private static decimal ValidatePositive(decimal? value, string message)
    {
        if (!value.HasValue || value.Value <= 0m)
        {
            throw new InvalidOperationException(message);
        }

        return RoundCurrency(value.Value);
    }

    private static decimal RoundCurrency(decimal amount)
    {
        return Math.Round(amount, 2, MidpointRounding.AwayFromZero);
    }

    private static FinancialTransaction CreateTransaction(Payment payment, Guid orderId, DateTime timestamp)
    {
        var description = payment.DriverId.HasValue
            ? $"Driver payout for order {orderId}"
            : $"Platform fee for order {orderId}";

        return new FinancialTransaction(Guid.NewGuid(), payment.Id, payment.Amount, description, timestamp);
    }
}
