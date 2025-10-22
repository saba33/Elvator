using System.Collections.Generic;
using TransportLink.Domain.Entities;
using TransportLink.Domain.Enums;

namespace TransportLink.Application.Matching;

public interface IMatchingService
{
    VehicleType SelectVehicleType(string cargoType, decimal weightKg);

    Driver? FindBestDriver(Order order, IReadOnlyCollection<Driver> availableDrivers);
}
