using System;
using System.Collections.Generic;
using TransportLink.Application.Matching;
using TransportLink.Domain.Entities;
using TransportLink.Domain.Enums;

namespace TransportLink.Infrastructure.Services;

public sealed class MatchingService : IMatchingService
{
    public VehicleType SelectVehicleType(string cargoType, decimal weightKg)
    {
        if (weightKg <= 1000)
        {
            return VehicleType.Pickup;
        }

        if (weightKg <= 5000)
        {
            return VehicleType.Van;
        }

        return VehicleType.Truck;
    }

    public Driver? FindBestDriver(Order order, IReadOnlyCollection<Driver> availableDrivers)
    {
        Driver? bestDriver = null;
        double bestScore = double.MinValue;

        foreach (var driver in availableDrivers)
        {
            var distanceScore = CalculateDistanceScore(order, driver);
            var workloadScore = CalculateWorkloadScore(driver);
            var ratingScore = CalculateRatingScore(driver);

            var totalScore = (0.4 * distanceScore) + (0.3 * workloadScore) + (0.3 * ratingScore);

            if (totalScore > bestScore)
            {
                bestScore = totalScore;
                bestDriver = driver;
            }
        }

        return bestDriver;
    }

    private static double CalculateDistanceScore(Order order, Driver driver)
    {
        var hash = Math.Abs(HashCode.Combine(order.CompanyId, driver.CompanyId, driver.Id));
        var distance = (hash % 9500) / 100.0; // value between 0 and 95 km
        var normalized = 1 - Math.Min(distance / 100.0, 1.0);
        return normalized;
    }

    private static double CalculateWorkloadScore(Driver driver)
    {
        var normalized = 1 - Math.Min(driver.ActiveAssignments / 5.0, 1.0);
        return normalized;
    }

    private static double CalculateRatingScore(Driver driver)
    {
        var normalized = Math.Clamp((double)driver.Rating / 5.0, 0, 1);
        return normalized;
    }
}
