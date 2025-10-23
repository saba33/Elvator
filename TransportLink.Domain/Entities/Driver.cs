using System.Collections.Generic;
using TransportLink.Domain.Enums;

namespace TransportLink.Domain.Entities;

public sealed class Driver
{
    private Driver()
    {
    }

    public Driver(
        Guid id,
        Guid companyId,
        string name,
        string phone,
        decimal rating,
        bool isAvailable,
        VehicleType vehicleType,
        int activeAssignments = 0)
    {
        Id = id;
        CompanyId = companyId;
        Name = name;
        Phone = phone;
        Rating = rating;
        IsAvailable = isAvailable;
        VehicleType = vehicleType;
        ActiveAssignments = activeAssignments;
    }

    public Guid Id { get; init; }

    public Guid CompanyId { get; private set; }

    public Company? Company { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Phone { get; private set; } = string.Empty;

    public decimal Rating { get; private set; }

    public bool IsAvailable { get; private set; }

    public VehicleType VehicleType { get; private set; }

    public int ActiveAssignments { get; private set; }

    public ICollection<Order> Orders { get; private set; } = new List<Order>();

    public ICollection<Payment> Payments { get; private set; } = new List<Payment>();

    public void UpdateAvailability(bool isAvailable)
    {
        IsAvailable = isAvailable;
        if (isAvailable)
        {
            ActiveAssignments = 0;
        }
    }

    public void AssignToOrder()
    {
        if (!IsAvailable)
        {
            return;
        }

        IsAvailable = false;
        ActiveAssignments += 1;
    }

    public void CompleteAssignment()
    {
        if (ActiveAssignments > 0)
        {
            ActiveAssignments -= 1;
        }

        if (ActiveAssignments <= 0)
        {
            ActiveAssignments = 0;
            IsAvailable = true;
        }
    }
}
