using System.Linq;
using Microsoft.EntityFrameworkCore;
using TransportLink.Domain.Entities;
using TransportLink.Domain.Enums;
using TransportLink.Infrastructure.Persistence;
using TransportLink.Infrastructure.Services;
using Xunit;

namespace TransportLink.Tests.Finance;

public sealed class PaymentEngineTests
{
    private static PaymentEngine CreatePaymentEngine(out TransportLinkDbContext context)
    {
        var options = new DbContextOptionsBuilder<TransportLinkDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new TransportLinkDbContext(options);
        return new PaymentEngine(context);
    }

    [Fact]
    public void CalculateForOrder_PerOrder_ReturnsConfiguredPercentage()
    {
        var paymentEngine = CreatePaymentEngine(out var context);
        using (context)
        {
            var companyId = Guid.NewGuid();
            var driverId = Guid.NewGuid();
            var order = new Order(Guid.NewGuid(), companyId, "Chemicals", 10m, 200m, OrderStatus.Delivered, DateTime.UtcNow, DateTime.UtcNow, driverId);
            var driver = new Driver(driverId, companyId, "Driver One", "+1-555-0000", 4.5m, true, VehicleType.Truck);
            var config = new CompanyPayrollConfig(companyId, SalaryMode.PerOrder, 0.2500m, null, null, DateTime.UtcNow);

            var amount = paymentEngine.CalculateForOrder(order, driver, config);

            Assert.Equal(50m, amount);
        }
    }

    [Fact]
    public void CalculateForOrder_Daily_ReturnsDailyRate()
    {
        var paymentEngine = CreatePaymentEngine(out var context);
        using (context)
        {
            var companyId = Guid.NewGuid();
            var driverId = Guid.NewGuid();
            var order = new Order(Guid.NewGuid(), companyId, "General", 5m, 150m, OrderStatus.Delivered, DateTime.UtcNow, DateTime.UtcNow, driverId);
            var driver = new Driver(driverId, companyId, "Driver Two", "+1-555-0001", 4.0m, true, VehicleType.Van);
            var config = new CompanyPayrollConfig(companyId, SalaryMode.Daily, null, 180m, null, DateTime.UtcNow);

            var amount = paymentEngine.CalculateForOrder(order, driver, config);

            Assert.Equal(180m, amount);
        }
    }

    [Fact]
    public void CalculateForOrder_Monthly_ReturnsMonthlySalary()
    {
        var paymentEngine = CreatePaymentEngine(out var context);
        using (context)
        {
            var companyId = Guid.NewGuid();
            var driverId = Guid.NewGuid();
            var order = new Order(Guid.NewGuid(), companyId, "Groceries", 3m, 120m, OrderStatus.Delivered, DateTime.UtcNow, DateTime.UtcNow, driverId);
            var driver = new Driver(driverId, companyId, "Driver Three", "+1-555-0002", 4.8m, true, VehicleType.Pickup);
            var config = new CompanyPayrollConfig(companyId, SalaryMode.Monthly, null, null, 3200m, DateTime.UtcNow);

            var amount = paymentEngine.CalculateForOrder(order, driver, config);

            Assert.Equal(3200m, amount);
        }
    }

    [Fact]
    public void CalculateForOrder_Fixed_ReturnsMonthlySalary()
    {
        var paymentEngine = CreatePaymentEngine(out var context);
        using (context)
        {
            var companyId = Guid.NewGuid();
            var driverId = Guid.NewGuid();
            var order = new Order(Guid.NewGuid(), companyId, "Retail", 8m, 180m, OrderStatus.Delivered, DateTime.UtcNow, DateTime.UtcNow, driverId);
            var driver = new Driver(driverId, companyId, "Driver Four", "+1-555-0003", 4.1m, true, VehicleType.Truck);
            var config = new CompanyPayrollConfig(companyId, SalaryMode.Fixed, null, null, 2800m, DateTime.UtcNow);

            var amount = paymentEngine.CalculateForOrder(order, driver, config);

            Assert.Equal(2800m, amount);
        }
    }

    [Fact]
    public async Task SettleOrderPaymentAsync_PerOrder_CreatesDriverAndPlatformPayments()
    {
        var paymentEngine = CreatePaymentEngine(out var context);
        await using (context)
        {
            var companyId = Guid.NewGuid();
            var driverId = Guid.NewGuid();
            var orderId = Guid.NewGuid();

            var company = new Company(companyId, "Seed Logistics", "1 Main Street", "ops@seed.com");
            var driver = new Driver(driverId, companyId, "Driver Five", "+1-555-0004", 4.7m, true, VehicleType.Truck);
            var order = new Order(orderId, companyId, "Machinery", 12m, 400m, OrderStatus.Delivered, DateTime.UtcNow, DateTime.UtcNow, driverId);
            var config = new CompanyPayrollConfig(companyId, SalaryMode.PerOrder, 0.6000m, null, null, DateTime.UtcNow);

            context.Companies.Add(company);
            context.Drivers.Add(driver);
            context.Orders.Add(order);
            context.CompanyPayrollConfigs.Add(config);
            await context.SaveChangesAsync();

            var result = await paymentEngine.SettleOrderPaymentAsync(orderId);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value!.Count);

            var driverPayment = result.Value.Single(payment => payment.DriverId == driverId);
            var platformPayment = result.Value.Single(payment => payment.DriverId is null);

            Assert.Equal(240m, driverPayment.Amount);
            Assert.Equal(160m, platformPayment.Amount);

            var transactions = await context.FinancialTransactions.ToListAsync();
            Assert.Equal(2, transactions.Count);
            Assert.All(transactions, transaction => Assert.Equal(orderId, result.Value!.First(payment => payment.Id == transaction.PaymentId).OrderId));
        }
    }
}
