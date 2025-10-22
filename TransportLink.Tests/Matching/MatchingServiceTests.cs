using TransportLink.Domain.Entities;
using TransportLink.Domain.Enums;
using TransportLink.Infrastructure.Services;
using Xunit;

namespace TransportLink.Tests.Matching;

public sealed class MatchingServiceTests
{
    [Fact]
    public void SelectVehicleType_UsesWeightThresholds()
    {
        var service = new MatchingService();

        Assert.Equal(VehicleType.Pickup, service.SelectVehicleType("Documents", 500));
        Assert.Equal(VehicleType.Van, service.SelectVehicleType("Electronics", 2500));
        Assert.Equal(VehicleType.Truck, service.SelectVehicleType("Machinery", 8000));
    }

    [Fact]
    public void FindBestDriver_ReturnsDriverWithHighestScore()
    {
        var service = new MatchingService();
        var companyId = Guid.NewGuid();
        var order = new Order(Guid.NewGuid(), companyId, "General Cargo", 2500, 1500, OrderStatus.Pending, DateTime.UtcNow, DateTime.UtcNow);

        var driverPreferred = new Driver(Guid.NewGuid(), companyId, "Preferred", "+1-555-0111", 4.9m, true, VehicleType.Van, activeAssignments: 0);
        var driverBusy = new Driver(Guid.NewGuid(), companyId, "Busy", "+1-555-0222", 4.0m, true, VehicleType.Van, activeAssignments: 4);
        var driverFar = new Driver(Guid.NewGuid(), companyId, "Far", "+1-555-0333", 4.7m, true, VehicleType.Van, activeAssignments: 1);

        var result = service.FindBestDriver(order, new[] { driverPreferred, driverBusy, driverFar });

        Assert.NotNull(result);
        Assert.Equal(driverPreferred.Id, result!.Id);
    }
}
